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
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Product
{
    public class GetProductCategory : BasePageRequest, IRequest<Result<IEnumerable<GetProductCategoryResponse>>>
    {
        public GetProductCategory(int page, int count)
        {
            this.Page = page;
            this.Count = count;
        }

        public GetProductCategory()
        {
            this.Page = 1;
            this.Count = 1000000;
        }
        /// <summary>
        /// Get Product category Handler.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<GetProductCategory, Result<IEnumerable<GetProductCategoryResponse>>>
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
            public async Task<Result<IEnumerable<GetProductCategoryResponse>>> Handle(GetProductCategory query, CancellationToken cancellationToken)
            {
                var products = await this.ThesisUnitOfWork.ProductCategories.GetByPageAndCount(query.Page, query.Count);

                var result = this.Mapper.Map<IEnumerable<GetProductCategoryResponse>>(products);
                return result.Any()
                    ? Result<IEnumerable<GetProductCategoryResponse>>.Success(result, SuccessMessage)
                    : Result<IEnumerable<GetProductCategoryResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
