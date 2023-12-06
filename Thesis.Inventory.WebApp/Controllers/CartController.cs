using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Inventory.ItemManagement.Services.Commands.Cart;
using Thesis.Inventory.ItemManagement.Services.Commands.Product;
using Thesis.Inventory.ItemManagement.Services.Queries.Cart;
using Thesis.Inventory.Shared.DTOs.Cart;
using Thesis.Inventory.Shared.DTOs.Product.Requests;

namespace Thesis.Inventory.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AddToCartRequest request)
        {
            var result = await this._mediator.Send(new AddToCart(request, User));
            return this.Ok(result);
        }

        [HttpGet]
        [Route("GetCart")]
        public async Task<IActionResult> GetCart()
        {
            var result = await this._mediator.Send(new GetAllCartByUserId(User));
            return this.Ok(result);
        }


        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this._mediator.Send(new DeleteCartItem(new DeleteCartItemRequest { CartId = id}, User));
            return this.Ok(result);
        }

        [HttpPost]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            var result = await this._mediator.Send(new CheckOut(request, User));
            return this.Ok(result);
        }

        [HttpPatch]
        [Route("AdjustQty")]
        public async Task<IActionResult> AdjustQty(AdjustQuantityRequest request)
        {
            var result = await this._mediator.Send(new ChangeQuantity(request, User));
            return this.Ok(result);
        }
    }
}
