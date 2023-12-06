using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.Extensions;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Product.Response;
using Thesis.Inventory.Shared.DTOs.Users.Responses;

namespace Thesis.Inventory.UserManagement.Services.Queries
{
    public class GetAllUsers :BasePageRequest, IRequest<Result<BasePageReponse<GetUserResponse>>>
    {
        public GetAllUsers(int page, int count)
        {
            this.Page = page;
            this.Count = count;
        }
        /// <summary>
        /// Get all users Handler.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<GetAllUsers, Result<BasePageReponse<GetUserResponse>>>
        {
            private const string SuccessMessage = "Successfully retrieved products.";
            private const string ErrorMessage = "products not found.";

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
            public async Task<Result<BasePageReponse<GetUserResponse>>> Handle(GetAllUsers query, CancellationToken cancellationToken)
            {
                var users = this.ThesisUnitOfWork.Users.Entities.ToList();

                var res_users = this.Mapper.Map<GetUserResponse[]>(users);
                var all_count = await this.ThesisUnitOfWork.Products.Count();
                var pagecount = (all_count / query.Count) + (all_count % query.Count) > 0 ? 1 : 0;

                var result = new BasePageReponse<GetUserResponse>();
                result.Data = res_users;
                result.CurrentPage = query.Page;
                result.PageCount = pagecount;

                return result != null
                    ? Result<BasePageReponse<GetUserResponse>>.Success(result, SuccessMessage)
                    : Result<BasePageReponse<GetUserResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
