using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Product
{
    public class SearchProduct : BasePageRequest, IRequest<Result<BasePageReponse<GetProductResponse>>>
    {
        public SearchProduct(int page, int count,string search)
        {
            this.Page = page;
            this.Count = count;
            this.Search = search;
        }
        public string Search { get; }

        /// <summary>
        /// Search product Handler.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<SearchProduct, Result<BasePageReponse<GetProductResponse>>>
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
            public async Task<Result<BasePageReponse<GetProductResponse>>> Handle(SearchProduct query, CancellationToken cancellationToken)
            {
                var products = await this.ThesisUnitOfWork.Products.GetAllAsync();
                var filtered = products.Where(x => x.ProductName.ToLower().Contains(query.Search) || x.ProductId == query.Search)
                    .Skip((query.Page - 1) * query.Count)
                    .Take(query.Count)
                    .ToList();

                var res_products = this.Mapper.Map<GetProductResponse[]>(filtered);
                var all_count = filtered.Count();
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
