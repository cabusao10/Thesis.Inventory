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

namespace Thesis.Inventory.ItemManagement.Services.Queries.Product
{
    public class GetLowStocks : IRequest<Result<GetLowStockResponse[]>>
    {
        public GetLowStocks()
        {

        }

        public class Handler : BaseService, IRequestHandler<GetLowStocks, Result<GetLowStockResponse[]>>
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
            public async Task<Result<GetLowStockResponse[]>> Handle(GetLowStocks query, CancellationToken cancellationToken)
            {
                var products = this.ThesisUnitOfWork.Products.Entities
                    .Where(x => x.Status != Shared.Enums.ProductStatusType.Archived && x.Status != Shared.Enums.ProductStatusType.Deleted)
                  .Where(x => x.Quantity < x.MinimumQuantity).ToList();

                var result = this.Mapper.Map<GetLowStockResponse[]>(products);

                return result != null
                    ? Result<GetLowStockResponse[]>.Success(result, SuccessMessage)
                    : Result<GetLowStockResponse[]>.Fail(ErrorMessage);
            }
        }
    }
}
