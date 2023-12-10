using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.UserManagement.Services.Commands;
using Thesis.Inventory.ItemManagement.Services.Queries.Product;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Product.Requests;
using Thesis.Inventory.ItemManagement.Services.Commands.Product;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Thesis.Inventory.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> SearchProduct(string search, int page)
        {
            var result = await this._mediator.Send(new SearchProduct(page, 100, search));
            return this.Ok(result);
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(int page)
        {
            var result = await this._mediator.Send(new GetProduct(page, 100));
            return this.Ok(result);
        }
    

        [HttpGet]
        [Route("GetArchives")]
        public async Task<IActionResult> GetArchives(int page)
        {
            var result = await this._mediator.Send(new GetArchivedProducts(page,100));
            return this.Ok(result);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await this._mediator.Send(new GetProductById(id));
            return this.Ok(result);
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories(int page)
        {
            var result = await this._mediator.Send(new GetProductCategory());
            return this.Ok(result);
        }

        [HttpGet]
        [Route("GetUom")]
        public async Task<IActionResult> GetUom(int page)
        {
            var result = await this._mediator.Send(new GetProductUom());
            return this.Ok(result);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            var result = await this._mediator.Send(new AddProduct(request));
            return this.Ok(result);
        }

        [HttpPut]
        [Route("Archive")]
        public async Task<IActionResult> Archive(ArchiveProductRequest request)
        {
            var result = await this._mediator.Send(new ArchiveProduct(request.ProductId));
            return this.Ok(result);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateProductRequest request)
        {
            var result = await this._mediator.Send(new UpdateProduct(request));
            return this.Ok(result);
        }

        [HttpPut]
        [Route("Delete")]
        public async Task<IActionResult> Delete(ArchiveProductRequest request)
        {
            var result = await this._mediator.Send(new DeleteProduct(request));
            return this.Ok(result);
        }


        [HttpPut]
        [Route("Restore")]
        public async Task<IActionResult> Restore(ArchiveProductRequest request)
        {
            var result = await this._mediator.Send(new RestoreProduct(request));
            return this.Ok(result);
        }

    }
}
