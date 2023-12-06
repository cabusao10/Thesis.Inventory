using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Order.Response;
using Thesis.Inventory.Shared.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Order
{
    public class GetOrdersByCustomerId :IRequest<Result<IEnumerable<OrderModel>>>
    {
        public GetOrdersByCustomerId(ClaimsPrincipal claims)
        {
            this.Claims = claims;
        }
        public ClaimsPrincipal Claims { get; }

        public class Handler : BaseService, IRequestHandler<GetOrdersByCustomerId, Result<IEnumerable<OrderModel>>>
        {
            private const string SuccessMessage = "Successfully retrieved products.";
            private const string ErrorMessage = "products not found.";
            private const string ErrorMessage2 = "User not found!";

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
            public async Task<Result<IEnumerable<OrderModel>>> Handle(GetOrdersByCustomerId query, CancellationToken cancellationToken)
            {
                if (query.Claims.Identity == null) return Result<IEnumerable<OrderModel>>.Fail(ErrorMessage2);

                var user = this.ThesisUnitOfWork.Users.Entities
                    .Where(x => x.Username == query.Claims.Identity.Name).FirstOrDefault();

                if (user == null)
                {
                    return Result<IEnumerable<OrderModel>>.Fail(SuccessMessage);
                }

                var orders = await this.ThesisUnitOfWork.Orders.Entities.Where(x=> x.UserId == user.Id).ToListAsync();

                var mapped_orders = this.Mapper.Map<OrderModel[]>(orders);
                //var result = mapped_orders.Select(x => new GetAllOrdersResponse
                //{
                //    Address = x.Customer.Address,
                //    CustomerName = x.Customer.Fullname,
                //    DateOrdered = x.DateCreated.ToString("MM/dd/yy HH:mm:ss"),
                //    Id = x.Id,
                //    PaymentMethod = x.PaymentMethod,
                //    PhoneNumber = x.Customer.ContactNumber,
                //    ProductName = x.Product.ProductName,
                //    Quantity = x.Quantity,
                //    Status = x.Status,
                //});

                return mapped_orders != null
                    ? Result<IEnumerable<OrderModel>>.Success(mapped_orders, SuccessMessage)
                    : Result<IEnumerable<OrderModel>>.Fail(ErrorMessage);
            }
        }
    }
}
