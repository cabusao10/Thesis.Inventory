using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Shared.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public OrderStatusType Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual UserModel Customer { get; set; }

    }
}
