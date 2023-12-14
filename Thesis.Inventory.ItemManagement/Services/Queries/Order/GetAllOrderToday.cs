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
using Thesis.Inventory.Shared.DTOs.Order.Response;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Order
{
    public class GetAllOrderToday : IRequest<Result<GetAllOrdersResponse[]>>
    {
        public GetAllOrderToday()
        {
            
        }
        public class Handler : BaseService, IRequestHandler<GetAllOrderToday, Result<GetAllOrdersResponse[]>>
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
            public async Task<Result<GetAllOrdersResponse[]>> Handle(GetAllOrderToday query, CancellationToken cancellationToken)
            {
                var orders = await this.ThesisUnitOfWork.Orders.GetAllAsync();
                orders = orders.Where(x => x.DateCreated.Value.Date == DateTime.Today.Date).ToList();

                var result = this.Mapper.Map<GetAllOrdersResponse[]>(orders);

                return result != null
                    ? Result<GetAllOrdersResponse[]>.Success(result, SuccessMessage)
                    : Result<GetAllOrdersResponse[]>.Fail(ErrorMessage);
            }
        }
    }
}
