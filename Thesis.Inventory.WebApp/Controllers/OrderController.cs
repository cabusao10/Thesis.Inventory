using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Inventory.ItemManagement.Services.Commands.Order;
using Thesis.Inventory.ItemManagement.Services.Queries.Cart;
using Thesis.Inventory.ItemManagement.Services.Queries.Order;
using Thesis.Inventory.Shared.DTOs.Order.Request;

namespace Thesis.Inventory.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(int page)
        {
            var result = await this._mediator.Send(new GetAllOrders(page,100));
            return this.Ok(result);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id,int page)
        {
            var result = await this._mediator.Send(new GetOrderById(id,page,100));
            return this.Ok(result);
        }

        [HttpGet]
        [Route("CustomerOrders")]
        public async Task<IActionResult> CustomerOrders()
        {
            var result = await this._mediator.Send(new GetOrdersByCustomerId(User));
            return this.Ok(result);
        }

        [HttpPatch]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(ChangeOrderStatusRequest request)
        {
            var result = await this._mediator.Send(new UpdateOrderStatus(request));
            return this.Ok(result);
        }
    }
}
