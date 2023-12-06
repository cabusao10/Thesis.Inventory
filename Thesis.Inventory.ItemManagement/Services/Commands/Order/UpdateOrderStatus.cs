using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.ItemManagement.Services.Commands.Cart;
using Thesis.Inventory.Shared.DTOs.Order.Request;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Order
{
    public class UpdateOrderStatus : IRequest<Result<bool>>
    {
        public UpdateOrderStatus(ChangeOrderStatusRequest request)
        {
            this.Request = request;    
        }

        public ChangeOrderStatusRequest Request { get; }

        public class Handler : BaseService, IRequestHandler<UpdateOrderStatus, Result<bool>>
        {
            private const string SuccessMessage = "Success changing order status.";
            private const string FailedMessage1 = "Failed  changing order status.";
            private const string FailedMessage2 = "Order not found.";

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
            public async Task<Result<bool>> Handle(UpdateOrderStatus command, CancellationToken cancellationToken)
            {
                try
                {
                    var request = command.Request;

                    var order = await this.ThesisUnitOfWork.Orders.GetByIdAsync(request.OrderId);
                    if (order == null)
                    {
                        return Result<bool>.Fail(FailedMessage2);
                    }
                    order.Status = request.NewStatus;

                    await this.ThesisUnitOfWork.Orders.UpdateAsync(order);
                    await this.ThesisUnitOfWork.Save();

                    return Result<bool>.Success(true, SuccessMessage);
                }
                catch (Exception ex)
                {
                    return Result<bool>.Fail(ex.Message);
                }
            }
        }
    }
}
