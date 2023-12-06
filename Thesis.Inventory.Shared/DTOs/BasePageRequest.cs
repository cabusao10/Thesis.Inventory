using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Shared.DTOs
{
    public class BasePageRequest
    {
        public int Page { get; set; }
        public int Count { get; set; }
    }
}
