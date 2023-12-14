using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Shared.DTOs.Users.Requests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Fullname { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int ProvinceId { get; set; }
        public DateTime Birthdate { get; set; }
        public int Role { get; set; }
    }
}
