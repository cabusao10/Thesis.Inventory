using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Domain.Entities
{
    public class ShoppingCartEntity : BaseEntity
    {
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual UserEntity Customer { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
