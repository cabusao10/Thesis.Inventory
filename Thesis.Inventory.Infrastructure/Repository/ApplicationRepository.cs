using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Infrastructure.Context;

namespace Thesis.Inventory.Infrastructure.Repository
{
    public class ApplicationRepository<T> : IApplicationRepository<T>
        where T : class
    {
        private readonly IThesisDBContext _dbContext;

        public ApplicationRepository(IThesisDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IQueryable<T> Entities => this._dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await this._dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task AddRangeAsync(T[] entities)
        {
            this._dbContext.Set<T>().AddRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(T[] entities)
        {
            this._dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await this._dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await this._dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await this._dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<int> Count()
        {
            return await this._dbContext.Set<T>().CountAsync();
        }
        public Task UpdateAsync(T entity)
        {
            this._dbContext.Set<T>().UpdateRange(entity);
            return Task.CompletedTask;
        }

        public Task UpdateRangeAsync(IQueryable<T> entities)
        {
            this._dbContext.Set<T>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this._dbContext.SaveChanges();
        }

        public async Task<List<T>> GetByPageAndCount(int page, int count)
        {
            return await this._dbContext.Set<T>().Skip(((page - 1) * count)).Take(count).ToListAsync();
        }
    }
}
