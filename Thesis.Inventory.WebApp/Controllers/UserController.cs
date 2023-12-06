using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.UserManagement.Services.Commands;
using Thesis.Inventory.UserManagement.Services.Queries;

namespace Thesis.Inventory.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {

            var result = await this._mediator.Send(new UserRegisterCommand(request));
            return this.Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var result = await this._mediator.Send(new LoginUserCommand(request));
            return this.Ok(result);
        }

        [HttpPost]
        [Route("MobileLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> MobileLogin(UserLoginRequest request)
        {
            var result = await this._mediator.Send(new MobileLogin(request));
            return this.Ok(result);
        }
        [HttpGet]
        [Route("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int page)
        {
            var result = await this._mediator.Send(new GetAllUsers(page,100));
            return this.Ok(result);
        }

        [HttpGet]
        [Route("GetUserById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await this._mediator.Send(new GetUserById(id));
            return this.Ok(result);
        }
        [HttpGet]
        [Route("GetProvinces")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProvinces()
        {
            var result = await this._mediator.Send(new GetProvincesQuery());
            return this.Ok(result);
        }
    }
}
