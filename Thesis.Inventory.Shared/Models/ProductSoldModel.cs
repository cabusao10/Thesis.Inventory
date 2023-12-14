using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Shared.Models
{
    public class ProductSoldModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
    }
}
