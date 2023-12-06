using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.UserManagement.Services.Commands
{
    public class UserRegisterCommand : IRequest<Result<Boolean>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegisterRequest"/> class.
        /// </summary>
        /// <param name="request">User register request.</param>
        public UserRegisterCommand(UserRegisterRequest request)
        {
            this.UserRegisterRequest = request ?? throw new ArgumentNullException(nameof(request));
        }

        /// <summary>
        /// Gets or sets request.
        /// </summary>
        public UserRegisterRequest UserRegisterRequest { get; set; }

        /// <summary>
        /// Handler for command.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<UserRegisterCommand, Result<Boolean>>
        {
            private const string SuccessMessage = "Success creating new account!";
            private const string FailedMessage1 = "Username or Email already exists.";
            private const string FailedMessage2 = "Password and Confirmpassword doesn't match";

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
            public async Task<Result<Boolean>> Handle(UserRegisterCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var request = command.UserRegisterRequest;

                    // Check if existing
                    var existinguser = this.ThesisUnitOfWork.Users.Entities.Where(x => x.Username == request.Username || x.Email == request.Email).FirstOrDefault();
                    if (existinguser != null)
                    {
                        return Result<Boolean>.Fail(FailedMessage1);
                    }

                    var newuser = this.Mapper.Map<UserEntity>(request);
                    newuser.DateCreated = DateTime.Now;

                    if(request.Password != request.ConfirmPassword)
                    {
                        return Result<Boolean>.Fail(FailedMessage2);
                    }
                    var encrpyted_password = ComputeHash(request.Password, new SHA256CryptoServiceProvider());
                    newuser.Password = encrpyted_password;
                    
                    await this.ThesisUnitOfWork.Users.AddAsync(newuser);
                    await this.ThesisUnitOfWork.Save();
                    return Result<Boolean>.Success(true, SuccessMessage);
                }
                catch (Exception ex)
                {
                    return Result<Boolean>.Fail(ex.Message);
                }
            }
            public string ComputeHash(string input, HashAlgorithm algorithm)
            {
                Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

                return BitConverter.ToString(hashedBytes).Replace("-","");
            }
        }

    }
}
