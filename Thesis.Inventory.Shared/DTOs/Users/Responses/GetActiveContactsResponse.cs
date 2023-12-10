using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Shared.DTOs.Users.Responses
{
    public class GetActiveContactsResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public bool HasLatestChat { get; set; }
    }
}
