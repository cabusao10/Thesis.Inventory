using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.Shared.DTOs.Cart;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.MobileApp.Model
{
    public partial class CartItemModel : ObservableObject
    {

        private readonly HttpClient _httpClient;
        public CartItemModel(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this.IsSelected = false;
        }


        public ProductModel Product { get; set; }
        [ObservableProperty]
        int quantity;

        [ObservableProperty]
        bool isSelected;

        [ObservableProperty]
        int id;

        [RelayCommand]
        async Task DeleteItem()
        {
            var request = new DeleteCartItemRequest
            {
                CartId = this.Id,
            };

            var response = await _httpClient.DeleteAsync<bool>($"Cart/Delete?id={this.Id}", request);
            if (response.Succeeded)
            {

                Toast.Make(response.Message);
            }
            else
            {
                Toast.Make(response.Message);

            }
        }
    }
}
