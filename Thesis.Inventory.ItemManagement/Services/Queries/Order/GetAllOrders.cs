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
using Thesis.Inventory.ItemManagement.Services.Queries.Product;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Order.Response;
using Thesis.Inventory.Shared.DTOs.Product.Response;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Order
{
    public class GetAllOrders : BasePageRequest, IRequest<Result<BasePageReponse<GetAllOrdersResponse>>>
    {
        public GetAllOrders(int page, int count)
        {
            this.Page = page;
            this.Count = count;
        }

        public class Handler : BaseService, IRequestHandler<GetAllOrders, Result<BasePageReponse<GetAllOrdersResponse>>>
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
            public async Task<Result<BasePageReponse<GetAllOrdersResponse>>> Handle(GetAllOrders query, CancellationToken cancellationToken)
            {
                var orders = await this.ThesisUnitOfWork.Orders.GetAllAsync();

                var mapped_orders = this.Mapper.Map<OrderModel[]>(orders);
                var response = mapped_orders.Select(x => new GetAllOrdersResponse
                {
                    Address = x.Customer.Address,
                    CustomerName = x.Customer.Fullname,
                    DateOrdered = x.DateCreated.ToString("MM/dd/yy HH:mm:ss"),
                    Id = x.Id,
                    PaymentMethod = x.PaymentMethod,
                    PhoneNumber = x.Customer.ContactNumber,
                    ProductName = x.Product.ProductName,
                    Quantity = x.Quantity,
                    Status = x.Status,
                });

                var all_count = await this.ThesisUnitOfWork.Products.Count();
                var pagecount = (all_count / query.Count) + (all_count % query.Count) > 0 ? 1 : 0;

                var result = new BasePageReponse<GetAllOrdersResponse>();
                result.Data = response;
                result.CurrentPage = query.Page;
                result.PageCount = pagecount;

                return result != null
                    ? Result<BasePageReponse<GetAllOrdersResponse>>.Success(result, SuccessMessage)
                    : Result<BasePageReponse<GetAllOrdersResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
