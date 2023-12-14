using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Shared.Models
{
    public class DashboardDetails
    {
        public int TotalItemSold { get; set; }
        public double TotalSales { get; set; }
        public double TotalSalesToday { get; set; }
        public double TotalSalesThisMonth { get; set; }
        public int TotalPaid { get; set; }
        public int TotalPending { get; set; }
        public int TotalConsumers { get; set; }
        public int ProductsCount { get; set; }

        public ProductSoldModel[] Top5 { get; set; }


    }
}
