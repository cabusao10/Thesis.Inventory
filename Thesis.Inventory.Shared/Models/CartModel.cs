using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Shared.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }
        public bool IsSelected { get; set; }
    }
}
