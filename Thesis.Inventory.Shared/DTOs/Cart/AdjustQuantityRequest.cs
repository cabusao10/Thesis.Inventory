using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Shared.DTOs.Cart
{
    public class AdjustQuantityRequest
    {
        public int CartId { get; set; }
        public int NewQuantity { get; set; }
    }
}
