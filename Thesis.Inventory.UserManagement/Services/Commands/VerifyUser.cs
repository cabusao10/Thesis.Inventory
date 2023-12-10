using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.UserManagement.Services.Commands
{
    public class VerifyUser : IRequest<Result<bool>>
    {
        public VerifyUser(VerifyUserRequest request, ClaimsPrincipal claims)
        {
            this.Request = request;
            this.Claims = claims;
        }
        public ClaimsPrincipal Claims { get; set; }
        public VerifyUserRequest Request { get; }

        public class Handler : BaseService, IRequestHandler<VerifyUser, Result<bool>>
        {
            private const string SuccessMessage = "Success verifying user.";
            private const string FailedMessage = "Failed to verify user.";

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="thesisUnitOfWork">User unit of work.</param>
            /// <param name="mapper">THe mapper.</param>
            public Handler(IThesisUnitOfWork thesisUnitOfWork, IMapper mapper, JwtSettings jwtSettings)
                : base(thesisUnitOfWork, mapper, jwtSettings)
            {
            }

            /// <inheritdoc/>
            public async Task<Result<bool>> Handle(VerifyUser command, CancellationToken cancellationToken)
            {
                try
                {

                    if (command.Claims.Identity == null) return Result<bool>.Fail(FailedMessage);

                    var request = command.Request;

                    var user = this.ThesisUnitOfWork.Users.Entities
                        .Where(x => x.Username == command.Claims.Identity.Name).FirstOrDefault();

                    if (user == null)
                    {
                        return Result<bool>.Fail(FailedMessage);
                    }
                    if (user.OTP != command.Request.OTP)
                    {
                        return Result<bool>.Fail(FailedMessage);
                    }
                    user.Status = Shared.Enums.UserStatusType.Verified;
                    await this.ThesisUnitOfWork.Save();

                    return Result<bool>.Success(true, SuccessMessage);

                }
                catch (Exception ex)
                {
                    return Result<bool>.Fail(ex.Message);
                }
            }
            public string ComputeHash(string input, HashAlgorithm algorithm)
            {
                Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

                return BitConverter.ToString(hashedBytes).Replace("-", "");
            }
        }
    }
}
