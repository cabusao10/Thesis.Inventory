using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Domain.Entities
{
    public class OrderEntity : BaseEntity
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Customer")]
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public OrderStatusType Status { get; set; }

        public virtual ProductEntity Product { get; set; }
        public virtual UserEntity Customer { get; set; }

    }
}
