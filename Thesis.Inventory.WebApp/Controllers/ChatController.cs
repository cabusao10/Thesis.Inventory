using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Inventory.UserManagement.Services.Queries;

namespace Thesis.Inventory.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetMessages")]
        public async Task<IActionResult> GetMessages(string user)
        {
            var result = await this._mediator.Send(new GetAllMessages(user, User));
            return this.Ok(result);
        }

        [HttpGet]
        [Route("ActiveContacts")]
        public async Task<IActionResult> ActiveContacts()
        {
            var result = await this._mediator.Send(new GetActiveContacts(User));
            return this.Ok(result);
        }
    }
}
