using System.ComponentModel.DataAnnotations.Schema;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Domain.Entities
{
    public class ProductEntity : BaseEntity
    {
        [Column("ProductId", TypeName = "nvarchar(300)")]
        public string ProductId { get; set; }

        [Column("ProductName", TypeName = "nvarchar(300)")]
        public string ProductName { get; set;}

        [ForeignKey("Category")]
        [Column("CategoryId", TypeName = "int")]
        public int CategoryId { get; set; }

        [ForeignKey("UOM")]
        [Column("UOMId", TypeName = "int")]
        public int UOMId { get; set; }

        [Column("Price", TypeName = "float")]
        public double Price { get; set; }

        [Column("Quantity", TypeName = "int")]
        public int Quantity { get; set; }
        [Column("MinimumQuantity", TypeName = "int")]
        public int MinimumQuantity { get; set; }

        [Column("Status", TypeName = "int")]
        public ProductStatusType Status { get; set; }

        public byte[]? Image { get; set; }
        public string? ImageType { get; set; }
        public virtual ProductCategoryEntity Category { get; set; }
        public virtual UOMEntity UOM { get; set; }

    }

}
