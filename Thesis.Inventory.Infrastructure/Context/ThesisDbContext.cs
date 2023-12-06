using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Domain.Entities;

namespace Thesis.Inventory.Infrastructure.Context
{
    public class ThesisDbContext : DbContext, IThesisDBContext
    {
        public ThesisDbContext(DbContextOptions<ThesisDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CompanyEntity> CompanySetting { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProvinceEntity> Provinces { get; set; }
        public DbSet<UOMEntity> UOM { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ShoppingCartEntity> ShoppingCarts { get; set; }

        public async Task<int> SaveChanges()
        {
            return await this.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
