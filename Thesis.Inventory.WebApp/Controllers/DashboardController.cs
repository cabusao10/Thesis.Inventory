using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thesis.Inventory.ItemManagement.Services.Commands.Report;
using Thesis.Inventory.ItemManagement.Services.Queries.Dashboard;
using Thesis.Inventory.ItemManagement.Services.Queries.Product;
using Thesis.Inventory.Shared.DTOs.Product.Requests;

namespace Thesis.Inventory.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("LowStocks")]
        public async Task<IActionResult> Add()
        {
            var result = await this._mediator.Send(new GetLowStocks());
            return this.Ok(result);
        }

        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> Details()
        {
            var result = await this._mediator.Send(new GetDashBoardDetails());
            return this.Ok(result);
        }

        [HttpGet]
        [Route("Reports")]
        public async Task<IActionResult> Details(ExportReport.ReportType type)
        {
            var result = await this._mediator.Send(new ExportReport(type));
            var filename = type == ExportReport.ReportType.users ?
                $"User Reports_{DateTime.Now.ToString()}.csv" :
                (type == ExportReport.ReportType.orders ? $"Orders Reports_{DateTime.Now.ToString()}.csv" 
                : $"Sales Reports_{DateTime.Now.ToString()}.csv");
            return this.File(result, "txt/csv", filename);
        }
    }
}
