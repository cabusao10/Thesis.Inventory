﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Shared.DTOs.Users.Responses
{
    public class GetUserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int ProvinceId { get; set; }
        public DateTime Birthdate { get; set; }
        public GenderType Gender { get; set; }
        public UserRoleType Role { get; set; }
        public UserStatusType Status { get; set; }
    }
}
