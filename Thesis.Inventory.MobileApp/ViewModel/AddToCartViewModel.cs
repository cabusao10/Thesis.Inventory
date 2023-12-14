using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;

using Thesis.Inventory.Shared.DTOs.Product.Requests;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class AddToCartViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly CartViewModel _cartViewModel;
        public AddToCartViewModel(HttpClient httpClient , CartViewModel carvm)
        {
            this._httpClient = httpClient;
            this._cartViewModel = carvm;
        }


        [ObservableProperty]
        int stock = 1;

        [ObservableProperty]
        int qty = 0;

        [ObservableProperty]
        int max = 1000000000;

        [ObservableProperty]
        double price = 1;

        [ObservableProperty]
        int productId;

        
       public async Task<bool> AddToCart()
        {
            var request = new AddToCartRequest
            {
                ProductId = this.ProductId,
                Quantity = this.Qty,
            };

            var result = await _httpClient.PostAsync<bool>("cart/add", request);
            this._cartViewModel.GetCarts();
            return result.Succeeded;
        }
    }
}
