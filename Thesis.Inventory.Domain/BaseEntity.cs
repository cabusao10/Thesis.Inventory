using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Domain
{
    public class BaseEntity
    {
        [Key]
        [Column("Id", TypeName = "int")]
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
