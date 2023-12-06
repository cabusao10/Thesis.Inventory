using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Shared.DTOs.Cart
{
    public class CheckoutRequest
    {
        public int[] CartIds { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
