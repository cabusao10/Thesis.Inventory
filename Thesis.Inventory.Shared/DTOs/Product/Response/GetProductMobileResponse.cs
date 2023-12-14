﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.Shared.DTOs.Product.Response
{
    public class GetProductMobileResponse
    {

        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int UOMId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageType { get; set; }
        public ProductStatusType Status { get; set; }
    }
}
