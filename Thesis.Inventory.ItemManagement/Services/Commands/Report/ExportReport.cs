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
using Thesis.Inventory.ItemManagement.Services.Queries.Product;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Report
{
    public class ExportReport : IRequest<byte[]>
    {
        public enum ReportType
        {
            orders,
            users,
            sales,
        }
        public ExportReport(ReportType reportType)
        {
            this.Type = reportType;
        }
        public ReportType Type { get; set; }
        public class Handler : BaseService, IRequestHandler<ExportReport, byte[]>
        {
            private const string SuccessMessage = "Successfully retrieved products.";
            private const string ErrorMessage = "products not found.";

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
            public async Task<byte[]> Handle(ExportReport query, CancellationToken cancellationToken)
            {
                var users = this.ThesisUnitOfWork.Users.Entities.Where(x => x.Status == Shared.Enums.UserStatusType.Verified).ToArray();
                var orders = this.ThesisUnitOfWork.Orders.Entities.ToArray();

                if (query.Type == ReportType.orders)
                {
                    return GetProductReport(orders);
                }
                else if (query.Type == ReportType.users)
                {
                    return GetUsersReport(users);
                }
                else if(query.Type == ReportType.sales)
                {
                    return GetSalesReport(orders);
                }

                return new byte[0];
            }

            public byte[] GetSalesReport(OrderEntity[] orders)
            {
                var monthly = orders.GroupBy(x => x.DateCreated.Value.Date).Select(x =>
                new
                {
                    Date = x.Key.ToShortDateString(),
                    TotalTransactions = x.Count(),
                    TotalSales = x.Sum(z => z.Quantity * z.Product.Price),
                }
                ).ToList();

                var csv = new StringBuilder();
                var csv_header = $"Date,Total Transactions,Total Sales";

                csv.AppendLine(csv_header);

                var csv_content = monthly.Select(x =>
                          $"{x.Date},{x.TotalTransactions},{x.TotalSales}"
                ).ToList();
                csv_content.ForEach(x => csv.AppendLine(x));
                var csv_footer = $",Total,{monthly.Sum(x => x.TotalSales)}";

                csv.AppendLine(csv_footer);

                using (var stream = GetStream(csv.ToString()))
                {
                    return stream.ToArray();
                }
            }

            private byte[] GetUsersReport(UserEntity[] users)
            {
                var csv = new StringBuilder();
                var csv_header = $"Id,Username,Fullname,Email,Gender,Address,Status";

                csv.AppendLine(csv_header);
                var csv_content =
                     users.Select(x => $"{x.Id},{x.Username},{x.Fullname}," +
                     $"{x.Email},{x.Gender.ToString()},\"{x.Address}\",{x.Status.ToString()}").ToList();

                csv_content.ForEach(x => csv.AppendLine(x));

                using (var stream = GetStream(csv.ToString()))
                {
                    return stream.ToArray();
                }


            }
            private byte[] GetProductReport(OrderEntity[] orders)
            {
                var csv = new StringBuilder();
                var csv_header = $"Date,Product Id,Product Name,Price,Quantity,Total,Status";

                csv.AppendLine(csv_header);
                var csv_content =
                     orders.Select(x => $"{x.DateCreated.Value.ToString("MM/dd/yy")}," +
                     $"{x.ProductId},{x.Product.ProductName},{x.Product.Price}," +
                     $"{x.Quantity},{x.Quantity * x.Product.Price},{x.Status.ToString()}").ToArray().ToList();

                csv_content.ForEach(x => csv.AppendLine(x));

                using (var stream = GetStream(csv.ToString()))
                {
                    return stream.ToArray();
                }


            }
            public MemoryStream GetStream(string content)
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(content);
                writer.Flush();
                stream.Position = 0;

                return stream;
            }
        }
    }
}
