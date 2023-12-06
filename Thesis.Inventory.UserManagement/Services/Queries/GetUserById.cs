using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Users.Responses;

namespace Thesis.Inventory.UserManagement.Services.Queries
{
    public class GetUserById : IRequest<Result<GetUserResponse>>
    {
        public GetUserById(int id)
        {
            this.UserId = id;
        }
        public int UserId { get; }

        /// <summary>
        /// Get all users Handler.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<GetUserById, Result<GetUserResponse>>
        {
            private const string SuccessMessage = "Successfully retrieved user.";
            private const string ErrorMessage = "User not found.";

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
            public async Task<Result<GetUserResponse>> Handle(GetUserById query, CancellationToken cancellationToken)
            {
                var user = this.ThesisUnitOfWork.Users.Entities.Where(x=> x.Id == query.UserId).FirstOrDefault();
                if(user == null)
                {
                  Result<GetUserResponse>.Fail(ErrorMessage);
                }

                var result = this.Mapper.Map<GetUserResponse>(user);

                return result != null
                    ? Result<GetUserResponse>.Success(result, SuccessMessage)
                    : Result<GetUserResponse>.Fail(ErrorMessage);
            }
        }
    }
}
