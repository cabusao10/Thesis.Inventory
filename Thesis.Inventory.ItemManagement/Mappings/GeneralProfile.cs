using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Shared.DTOs.Order.Response;
using Thesis.Inventory.Shared.DTOs.Product.Requests;
using Thesis.Inventory.Shared.DTOs.Product.Response;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.ItemManagement.Mappings
{
    public class GeneralProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralProfile"/> class.
        /// </summary>
        public GeneralProfile()
        {
            this.CreateMap<ProductEntity, GetProductResponse>().ReverseMap();
            this.CreateMap<ProductEntity[], GetProductResponse[]>().ReverseMap();
            this.CreateMap<ProductEntity, AddProductRequest>().ReverseMap();
            this.CreateMap<ShoppingCartEntity, CartModel>().ReverseMap();
            this.CreateMap<ProductModel, ProductEntity>().ReverseMap();
            this.CreateMap<OrderModel, OrderEntity>().ReverseMap();
            this.CreateMap<GetAllOrdersResponse, OrderEntity>().ReverseMap();
            this.CreateMap<OrderModel[], OrderEntity[]>().ReverseMap();
            this.CreateMap<UserModel, UserEntity>().ReverseMap();
            this.CreateMap<ProductEntity, GetLowStockResponse>().ReverseMap();
            this.CreateMap<AddUomRequest, UOMEntity>().ReverseMap();


            this.CreateMap<UOMEntity, GetUomResponse>().ReverseMap();

            this.CreateMap<ProductCategoryEntity, AddProductCategoryRequest>().ReverseMap();
            this.CreateMap<ProductCategoryEntity, GetProductCategoryResponse>().ReverseMap();
        }
    }
}
