using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
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

namespace Thesis.Inventory.ItemManagement.Services.Queries.Product
{
    public class GetProductMobile : BasePageRequest, IRequest<Result<BasePageReponse<GetProductMobileResponse>>>
    {
        public GetProductMobile(int page, int count)
        {
            this.Page = page;
            this.Count = count;
        }

        /// <summary>
        /// Get Products Handler.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<GetProductMobile, Result<BasePageReponse<GetProductMobileResponse>>>
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
            public async Task<Result<BasePageReponse<GetProductMobileResponse>>> Handle(GetProductMobile query, CancellationToken cancellationToken)
            {
                var products = this.ThesisUnitOfWork.Products.Entities
                    .Where(x => x.Status != Shared.Enums.ProductStatusType.Archived && x.Status != Shared.Enums.ProductStatusType.Deleted).GetPaged(query.Page, 100).ToList();
                var res_products = this.Mapper.Map<GetProductMobileResponse[]>(products);
                var all_count = await this.ThesisUnitOfWork.Products.Count();
                var pagecount = (all_count / query.Count) + (all_count % query.Count) > 0 ? 1 : 0;

                var result = new BasePageReponse<GetProductMobileResponse>();
                result.Data = res_products;
                result.CurrentPage = query.Page;
                result.PageCount = pagecount;

                return result != null
                    ? Result<BasePageReponse<GetProductMobileResponse>>.Success(result, SuccessMessage)
                    : Result<BasePageReponse<GetProductMobileResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
