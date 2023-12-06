using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Product.Requests;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Product
{
    public class AddToCart : IRequest<Result<bool>>
    {
        public AddToCart(AddToCartRequest request, ClaimsPrincipal claims)
        {
            this.Request = request;
            this.Claims = claims;
        }
        public AddToCartRequest Request { get; }
        public ClaimsPrincipal Claims { get; }

        /// <summary>
        /// Handler for command.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<AddToCart, Result<bool>>
        {
            private const string SuccessMessage = "Success adding to cart!";
            private const string FailedMessage1 = "Failed adding to cart.";
            private const string FailedMessage2 = "Consumer not found";

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
            public async Task<Result<bool>> Handle(AddToCart command, CancellationToken cancellationToken)
            {
                try
                {
                    if (command.Claims.Identity == null) return Result<bool>.Fail(FailedMessage2);

                    var request = command.Request;

                    var user = this.ThesisUnitOfWork.Users.Entities
                        .Where(x => x.Username == command.Claims.Identity.Name).FirstOrDefault();

                    if (user == null)
                    {
                        return Result<bool>.Success(true, SuccessMessage);
                    }

                    // look for existing cart
                    var existingCart = await this.ThesisUnitOfWork.ShoppingCarts
                        .Entities.Where(x => x.CustomerId == user.Id && x.ProductId == request.ProductId)
                        .FirstOrDefaultAsync();

                    if(existingCart == null)
                    {
                        var cart = new ShoppingCartEntity
                        {
                            ProductId = request.ProductId,
                            CustomerId = user.Id,
                            Quantity = request.Quantity,
                            DateCreated = DateTime.Now,
                        };

                        await this.ThesisUnitOfWork.ShoppingCarts.AddAsync(cart);
                    }
                    else
                    {
                        existingCart.Quantity += request.Quantity;
                    }

                   
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
