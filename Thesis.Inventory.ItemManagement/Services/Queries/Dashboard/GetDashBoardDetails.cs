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
using Thesis.Inventory.ItemManagement.Services.Queries.Order;
using Thesis.Inventory.Shared.DTOs.Order.Response;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Dashboard
{
    public class GetDashBoardDetails : IRequest<Result<DashboardDetails>>
    {
        public GetDashBoardDetails()
        {

        }

        public class Handler : BaseService, IRequestHandler<GetDashBoardDetails, Result<DashboardDetails>>
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
            public async Task<Result<DashboardDetails>> Handle(GetDashBoardDetails query, CancellationToken cancellationToken)
            {
                var result = new DashboardDetails();

                result.TotalConsumers = await this.ThesisUnitOfWork.Users.Entities.CountAsync(x => x.Status == Shared.Enums.UserStatusType.Verified && x.Role == Shared.Enums.UserRoleType.Consumer);
                result.ProductsCount = await this.ThesisUnitOfWork.Products
                    .Entities.CountAsync(x => x.Status == Shared.Enums.ProductStatusType.Active);

                var top5 = this.ThesisUnitOfWork.Orders.Entities.GroupBy(x => x.Product)
                    .Select(x => new
                    {
                        Product = x.Key,
                        TotalCount = x.Count()
                    }).OrderByDescending(x=> x.TotalCount).Take(5).ToArray();

                result.Top5 = top5.Select(x => new ProductSoldModel
                {
                    ProductId = x.Product.ProductId,
                    ProductName = x.Product.ProductName,
                    TotalSold = x.TotalCount
                }).ToArray();

                result.TotalSales = await this.ThesisUnitOfWork.Orders.Entities.SumAsync(x => x.Quantity * x.Product.Price);
                result.TotalSalesToday = await this.ThesisUnitOfWork.Orders.Entities.Where(x => x.DateCreated.HasValue)
                    .Where(x => x.DateCreated.Value.Date == DateTime.Now.Date).SumAsync(x => x.Quantity * x.Product.Price);
                result.TotalSalesThisMonth = await this.ThesisUnitOfWork.Orders.Entities.Where(x => x.DateCreated.HasValue)
                    .Where(x => x.DateCreated.Value.Month == DateTime.Now.Month && x.DateCreated.Value.Year == DateTime.Now.Year)
                    .SumAsync(x => x.Quantity * x.Product.Price);

                result.TotalItemSold = await this.ThesisUnitOfWork.Orders.Entities.SumAsync(x => x.Quantity);

                result.TotalPending = await this.ThesisUnitOfWork.Orders.Entities.CountAsync(x => x.Status != Shared.Enums.OrderStatusType.Delivered && x.Status != Shared.Enums.OrderStatusType.Cancelled);
                result.TotalPaid = await this.ThesisUnitOfWork.Orders.Entities.CountAsync(x => x.Status == Shared.Enums.OrderStatusType.Delivered);


                return result != null
                    ? Result<DashboardDetails>.Success(result, SuccessMessage)
                    : Result<DashboardDetails>.Fail(ErrorMessage);
            }
        }
    }
}
