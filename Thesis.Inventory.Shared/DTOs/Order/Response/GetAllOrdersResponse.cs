using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Shared.DTOs.Order.Response
{
    public class GetAllOrdersResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public string DateOrdered { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public OrderStatusType Status { get; set; }
    }
}
