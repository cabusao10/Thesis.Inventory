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
using Thesis.Inventory.Shared.DTOs.Cart;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Cart
{
    public class ChangeQuantity : IRequest<Result<bool>>
    {
        public ChangeQuantity(AdjustQuantityRequest request , ClaimsPrincipal claims)
        {
            this.Request = request;
            this.Claims = claims;
        }
        public AdjustQuantityRequest Request { get; }
        public ClaimsPrincipal Claims { get;}

        public class Handler : BaseService, IRequestHandler<ChangeQuantity, Result<bool>>
        {
            private const string SuccessMessage = "Success adjusting item in cart.";
            private const string FailedMessage1 = "Failed adjusting item in cart.";
            private const string FailedMessage2 = "Cart item not found.";

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
            public async Task<Result<bool>> Handle(ChangeQuantity command, CancellationToken cancellationToken)
            {
                try
                {
                    if (command.Claims.Identity == null) return Result<bool>.Fail(FailedMessage2);

                    var request = command.Request;

                    var user = this.ThesisUnitOfWork.Users.Entities
                        .Where(x => x.Username == command.Claims.Identity.Name).FirstOrDefault();

                    if (user == null)
                    {
                        return Result<bool>.Fail(FailedMessage2);
                    }

                    // look for existing cart
                    var existingCart = await this.ThesisUnitOfWork.ShoppingCarts
                        .Entities.Where(x => x.CustomerId == user.Id && x.Id == request.CartId)
                        .FirstOrDefaultAsync();

                    if (existingCart == null)
                    {
                        return Result<bool>.Fail(FailedMessage1);
                    }
                    existingCart.Quantity = request.NewQuantity;

                    await this.ThesisUnitOfWork.ShoppingCarts.UpdateAsync(existingCart);

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
