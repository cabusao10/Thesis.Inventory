using System.ComponentModel.DataAnnotations.Schema;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string? Businessname { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        public DateTime Birthdate { get; set; }
        public GenderType Gender { get; set; }
        public UserRoleType Role { get; set; }
        public UserStatusType Status { get; set; }
        public string? OTP { get; set; }
        public string? MessageConnectionId { get; set; }
        public virtual ProvinceEntity Province { get; set; }
        
    }
}
