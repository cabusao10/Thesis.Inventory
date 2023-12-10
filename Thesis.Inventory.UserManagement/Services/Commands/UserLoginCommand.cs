using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Thesis.Inventory.Authentication.Extensions;

namespace Thesis.Inventory.UserManagement.Services.Commands
{
    public class LoginUserCommand : IRequest<Result<UserLoginResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserCommand"/> class.
        /// </summary>
        /// <param name="request">Login request.</param>
        public LoginUserCommand(UserLoginRequest request)
        {
            this.LoginUserRequest = request ?? throw new ArgumentNullException(nameof(request));
        }

        /// <summary>
        /// Gets or sets request.
        /// </summary>
        public UserLoginRequest LoginUserRequest { get; set; }

        /// <summary>
        /// Handler for command.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<LoginUserCommand, Result<UserLoginResponse>>
        {
            private const string SuccessMessage = "Success logging in.";
            private const string FailedMessage = "Invalid username or password.";

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
            public async Task<Result<UserLoginResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
            {
                try
                {

                    var request = command.LoginUserRequest;

                    var encrpyted_password = ComputeHash(command.LoginUserRequest.Password, new SHA256CryptoServiceProvider());

                    var user = await this.ThesisUnitOfWork.Users.Entities.FirstOrDefaultAsync(x => x.Role != Shared.Enums.UserRoleType.Consumer && x.Username == request.Username && x.Password == encrpyted_password);

                    if (user != null)
                    {
                        var response = this.Mapper.Map(user, new UserLoginResponse());

                        var useridbytes = new byte[16];
                        BitConverter.GetBytes(user.Id).CopyTo(useridbytes, 0);

                        var usertoken = new UserTokenModel
                        {
                            EmailId = user.Email,
                            GuidId = Guid.NewGuid(),
                            UserName = user.Username,
                            Id = new Guid(useridbytes),
                        }.GenTokenkey(this.JwtSettings);

                        response.BearerToken = usertoken.Token;
                        return Result<UserLoginResponse>.Success(response, SuccessMessage);
                    }
                    else
                    {
                        return Result<UserLoginResponse>.Fail(FailedMessage);
                    }

                }
                catch (Exception ex)
                {
                    return Result<UserLoginResponse>.Fail(ex.Message);
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
