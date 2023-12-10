using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Domain.Entities;

namespace Thesis.Inventory.Infrastructure.Context
{
    public interface IThesisDBContext
    {
        DbSet<UserEntity> Users { get; set; }
        DbSet<CompanyEntity> CompanySetting { get; set; }
        DbSet<ProductEntity> Products { get; set; }
        DbSet<ProvinceEntity> Provinces { get; set; }
        DbSet<UOMEntity>UOM { get; set; }
        DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        DbSet<OrderEntity> Orders { get; set; }
        DbSet<ChatRoomEntity> ChatRooms { get; set; }
        DbSet<ChatMessageEntity> ChatRoomMessages { get; set; }
        Task<int> SaveChanges();
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;
    }
}
