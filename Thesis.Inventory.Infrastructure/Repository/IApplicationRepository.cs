using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.Infrastructure.Repository
{
    public interface IApplicationRepository<T>
        where T : class
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync();
        Task<List<T>> GetByPageAndCount(int page, int count);

        Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize);

        Task<int> Count();

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(T[] entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(T[] entities);

        Task UpdateRangeAsync(IQueryable<T> entities);
        
        Task<int> SaveChangesAsync();
    }
}
