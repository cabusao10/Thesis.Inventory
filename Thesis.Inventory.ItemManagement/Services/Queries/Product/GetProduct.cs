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
    public class GetProduct : BasePageRequest, IRequest<Result<BasePageReponse<GetProductResponse>>>
    {
        public GetProduct(int page, int count)
        {
            this.Page = page;
            this.Count = count;
        }

        /// <summary>
        /// Get Products Handler.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<GetProduct, Result<BasePageReponse<GetProductResponse>>>
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
            public async Task<Result<BasePageReponse<GetProductResponse>>> Handle(GetProduct query, CancellationToken cancellationToken)
            {
                var products = this.ThesisUnitOfWork.Products.Entities
                    .Where(x => x.Status != Shared.Enums.ProductStatusType.Archived && x.Status != Shared.Enums.ProductStatusType.Deleted)
                    .Where(x => x.Quantity > 0).GetPaged(query.Page, 100).ToList();
                var res_products = this.Mapper.Map<GetProductResponse[]>(products);

                for(int x = 0; x< res_products.Length; x++)
                {
                    res_products[x].IsLowStock = res_products[x].Quantity < res_products[x].MinimumQuantity;
                }

                var all_count = await this.ThesisUnitOfWork.Products.Count();
                var pagecount = (all_count / query.Count) + (all_count % query.Count) > 0 ? 1 : 0;

                var result = new BasePageReponse<GetProductResponse>();
                result.Data = res_products;
                result.CurrentPage = query.Page;
                result.PageCount = pagecount;

                return result != null
                    ? Result<BasePageReponse<GetProductResponse>>.Success(result, SuccessMessage)
                    : Result<BasePageReponse<GetProductResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
