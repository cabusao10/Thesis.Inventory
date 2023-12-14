using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Users.Requests;

namespace Thesis.Inventory.UserManagement.Services.Commands
{
    public class UpdateUser : IRequest<Result<bool>>
    {
        public UpdateUser(UpdateUserRequest request)
        {
            this.Request = request;
        }
        public UpdateUserRequest Request { get; set; }
        public class Handler : BaseService, IRequestHandler<UpdateUser, Result<bool>>
        {
            private const string SuccessMessage = "Success updating user.";
            private const string FailedMessage = "User not found.";

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
            public async Task<Result<bool>> Handle(UpdateUser command, CancellationToken cancellationToken)
            {
                try
                {
                    var request = command.Request;

                    var user = this.ThesisUnitOfWork.Users.Entities
                        .Where(x => x.Id == request.Id).FirstOrDefault();

                    if (user == null)
                    {
                        return Result<bool>.Fail(FailedMessage);
                    }

                    user.Birthdate = request.Birthdate;
                    user.Businessname = request.BusinessName;
                    user.Fullname = request.Fullname;
                    user.Role = (Shared.Enums.UserRoleType) request.Role;
                    //user.Gender = request.Gender;
                    user.ContactNumber = request.ContactNumber;
                    user.Address = request.Address;
                    user.ProvinceId = request.ProvinceId;
                    user.ZipCode = request.ZipCode;


                    await this.ThesisUnitOfWork.Users.UpdateAsync(user);
                    await this.ThesisUnitOfWork.Save();

                    return Result<bool>.Success(true, SuccessMessage);

                }
                catch (Exception ex)
                {
                    return Result<bool>.Fail(ex.Message);
                }
            }
        }
    }
}
