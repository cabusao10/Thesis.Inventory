using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Shared.DTOs
{
    public class BasePageReponse<T>
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
