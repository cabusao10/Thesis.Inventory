using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Cart
{
    public class GetAllCartByUserId : IRequest<Result<CartModel[]>>
    {
        public GetAllCartByUserId(ClaimsPrincipal claims)
        {
            this.Claims = claims;   
        }

        public ClaimsPrincipal Claims { get; }
        /// <summary>
        /// Handler for query.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<GetAllCartByUserId, Result<CartModel[]>>
        {
            private const string SuccessMessage = "Success getting all the cart.";
            private const string FailedMessage1 = "Failed to fetch the cart.";
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
            public async Task<Result<CartModel[]>> Handle(GetAllCartByUserId command, CancellationToken cancellationToken)
            {
                try
                {
                    if (command.Claims.Identity == null) return Result<CartModel[]>.Fail(FailedMessage2);

                 
                    var user = this.ThesisUnitOfWork.Users.Entities
                        .Where(x => x.Username == command.Claims.Identity.Name).FirstOrDefault();

                    if (user == null)
                    {
                        return Result<CartModel[]>.Fail(SuccessMessage);
                    }

                    // get all the cart
                    var cartsEntities =  this.ThesisUnitOfWork.ShoppingCarts.Entities.Where(x=> x.CustomerId == user.Id).ToArray();
                    var result = this.Mapper.Map<CartModel[]>(cartsEntities);

                    return Result<CartModel[]>.Success(result, SuccessMessage);
                }
                catch (Exception ex)
                {
                    return Result<CartModel[]>.Fail(ex.Message);
                }
            }
        }
    }
}
