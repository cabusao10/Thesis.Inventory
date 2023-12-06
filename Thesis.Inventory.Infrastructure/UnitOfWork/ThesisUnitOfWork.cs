using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.Context;
using Thesis.Inventory.Infrastructure.Repository;

namespace Thesis.Inventory.Infrastructure.UnitOfWork
{
    public class ThesisUnitOfWork : IThesisUnitOfWork
    {
        private readonly IThesisDBContext _dbContext;
        private IApplicationRepository<UserEntity> _userRepo;
        private IApplicationRepository<CompanyEntity> _companyRepo;
        private IApplicationRepository<ProductEntity> _productRepo;
        private IApplicationRepository<ProvinceEntity> _provinceRepo;
        private IApplicationRepository<UOMEntity> _uomRepo;
        private IApplicationRepository<ProductCategoryEntity> _productCategoryRepo;
        private IApplicationRepository<OrderEntity> _orderRepo;
        private IApplicationRepository<ShoppingCartEntity> _shoppingCartRepo;
        public ThesisUnitOfWork(IThesisDBContext dBContext,
            IApplicationRepository<UserEntity> userRepo,
            IApplicationRepository<CompanyEntity> companyRepo,
            IApplicationRepository<ProductEntity> productRepo,
            IApplicationRepository<ProvinceEntity> provinceRepo,
            IApplicationRepository<UOMEntity> uomRepo,
            IApplicationRepository<ProductCategoryEntity> productCategoryRepo,
            IApplicationRepository<OrderEntity> orderRepo,
            IApplicationRepository<ShoppingCartEntity> shoppingCartRepo)
        {
            _dbContext = dBContext;
            _userRepo = userRepo;
            _companyRepo = companyRepo;
            _productRepo = productRepo;
            _provinceRepo = provinceRepo;
            _uomRepo = uomRepo;
            _productCategoryRepo = productCategoryRepo;
            _orderRepo = orderRepo;
            _shoppingCartRepo = shoppingCartRepo;

        }
        public async Task<int> Save()
        {
            return await this._dbContext.SaveChanges();
        }
        public IApplicationRepository<UserEntity> Users { get { return _userRepo; } }
        public IApplicationRepository<CompanyEntity> CompanySetting { get { return _companyRepo; } }
        public IApplicationRepository<ProductEntity> Products { get { return _productRepo; } }
        public IApplicationRepository<ProvinceEntity> Provinces { get { return _provinceRepo; } }
        public IApplicationRepository<UOMEntity> UOM { get { return _uomRepo; } }
        public IApplicationRepository<ProductCategoryEntity> ProductCategories { get { return _productCategoryRepo; } }
        public IApplicationRepository<OrderEntity> Orders { get { return _orderRepo; } }
        public IApplicationRepository<ShoppingCartEntity> ShoppingCarts { get { return _shoppingCartRepo; } }

    }
}
