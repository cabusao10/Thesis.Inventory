using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Shared.DTOs.Order.Request
{
    public class ChangeOrderStatusRequest
    {
        public int OrderId { get; set; }
        public OrderStatusType NewStatus { get; set; }
    }
}
