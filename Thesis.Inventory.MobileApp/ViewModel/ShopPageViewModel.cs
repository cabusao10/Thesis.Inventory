using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.MobileApp.Pages;
using Thesis.Inventory.Shared.DTOs.Product.Response;
using Thesis.Inventory.Shared.DTOs;
using System.Net.Http.Headers;
using Thesis.Inventory.MobileApp.Model;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class ShopPageViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly IPopupService _popupService;
        public ShopPageViewModel(HttpClient httpClient, IPopupService popup)
        {
            _httpClient = httpClient;
            _popupService = popup;
            this.GetProducts();
        }
        public ShopProductViewModel TEst;

        [ObservableProperty]
        ObservableCollection<ShopProductViewModel> products;

        [ObservableProperty]
        ShopProductViewModel tmpproducts;

        [ObservableProperty]
        bool isRefreshing;

      

        [RelayCommand]
        async Task Refreshing()
        {
            this.IsRefreshing = false;
            this.GetProducts();
            await Task.CompletedTask;
        }
        [RelayCommand]
        async Task AddToCart()
        {
            this._popupService.ShowPopup<AddToCartViewModel>();
            await Task.CompletedTask;
        }
        private async void GetProducts()
        {
            var response = await _httpClient.GetAsync<BasePageReponse<GetProductResponse>>("product/getall?page=1");

            if (response.Succeeded)
            {
                //this.Products = response.Data.Data.Select(x => new ProductModel
                //{
                //    CategoryId = x.CategoryId,
                //    Id = x.Id,
                //    Image = ImageSource.FromStream(() => new MemoryStream(x.Image)),
                //    Price = x.Price,
                //    ProductId = x.ProductId,
                //    ProductName = x.ProductName,
                //    Quantity = x.Quantity,
                //    Status = x.Status,
                //    UOMId = x.UOMId,

                //}).ToList();

                this.Products = response.Data.Data.Select(x => new ShopProductViewModel(_httpClient, _popupService)
                {
                    CategoryId = x.CategoryId,
                    Id = x.Id,
                    Image = ImageSource.FromStream(() => new MemoryStream(x.Image)),
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    Status = x.Status,
                    UomId = x.UOMId,

                }).ToObservableCollection();
            }
            else
            {

            }
        }


    }
}
