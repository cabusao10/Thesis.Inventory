using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Infrastructure.Extensions
{
    public static class LinqExtensions
    {
        public static List<T> GetPaged<T>(this List<T> list, int pageNumber, int pageSize)
        {
            return list.Skip(((pageNumber - 1) * pageSize)).Take(pageSize).ToList();
        }
        public static IQueryable<T> GetPaged<T>(this IQueryable<T> list, int pageNumber, int pageSize)
        {
            return list.Skip(((pageNumber - 1) * pageSize)).Take(pageSize);
        }
    }
}
