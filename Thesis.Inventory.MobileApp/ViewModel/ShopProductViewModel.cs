using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class ShopProductViewModel : ObservableObject
    {

        private readonly HttpClient _httpClient;
        private readonly IPopupService _popupService;
        public ShopProductViewModel(HttpClient httpClient, IPopupService popup)
        {
            _httpClient = httpClient;
            _popupService = popup;
        }
        [ObservableProperty]
        byte[] productImage;

        [ObservableProperty]
        string productName;

        [ObservableProperty]
        double price;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string productId;

        [ObservableProperty]
        int categoryId;

        [ObservableProperty]
        int uomId;

        [ObservableProperty]
        int quantity;

        [ObservableProperty]
        ImageSource image;

        [ObservableProperty]
        ProductStatusType status;


        [RelayCommand]
        async Task AddToCart()
        {
            this._popupService.ShowPopup<AddToCartViewModel>(o =>
            {
                o.Stock = this.Quantity;
                o.Max = this.Quantity;
                o.Qty = 1;
                o.Price = this.Price;
                o.ProductId = this.Id;
            });
            await Task.CompletedTask;
        }
    }
}
