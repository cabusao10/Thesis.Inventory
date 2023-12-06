using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.Repository;

namespace Thesis.Inventory.Infrastructure.UnitOfWork
{
    public interface IThesisUnitOfWork
    {
        IApplicationRepository<UserEntity> Users {get;}
        IApplicationRepository<CompanyEntity> CompanySetting {get;}
        IApplicationRepository<ProductEntity> Products {get;}
        IApplicationRepository<ProvinceEntity> Provinces {get;}
        IApplicationRepository<UOMEntity> UOM {get;}
        IApplicationRepository<ProductCategoryEntity> ProductCategories {get;}
        IApplicationRepository<OrderEntity> Orders {get;}
        IApplicationRepository<ShoppingCartEntity> ShoppingCarts {get; }

        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> Save();
    }
}
