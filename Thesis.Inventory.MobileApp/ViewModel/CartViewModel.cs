using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.MobileApp.Model;
using Thesis.Inventory.Shared.DTOs.Cart;
using Thesis.Inventory.Shared.Enums;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    [QueryProperty(nameof(IsSuccessPayment), "isSuccessPayment")]
    public partial class CartViewModel : ObservableObject
    {
        public CartViewModel()
        {

        }
        private readonly HttpClient _httpClient;
        private readonly IPopupService _popupService;
        public CartViewModel(HttpClient httpClient, IPopupService popupService)
        {
            this._httpClient = httpClient;
            this._popupService = popupService;
            GetCarts();
        }
        [ObservableProperty]
        bool isSuccessPayment;
        [ObservableProperty]
        ObservableCollection<CartItemModel> carts;

        [ObservableProperty]
        bool thereIsSelected;

        [ObservableProperty]
        bool isEmptyVisible;

        [ObservableProperty]
        bool isCartItemVisible;

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task Refresh()
        {
            this.IsRefreshing = false;
            GetCarts();

            await Task.CompletedTask;
        }
        [RelayCommand]
        async Task CheckOut()
        {
            // check if there is selected
            var selected = this.Carts.Where(x => x.IsSelected);
            if (selected.Any())
            {

                var checkoutmodel = new CheckOutViewModel(_httpClient)
                {
                    TotalPrice = selected.Sum(x => x.Quantity * x.Product.Price),
                    CartId = selected.Select(x => x.Id).ToArray(),
                    PaymentType = "Cash on Delivery",
                };

                this._popupService.ShowPopup<CheckOutViewModel>(onPresenting: vm =>
                {
                    vm.TotalPrice = selected.Sum(x => x.Quantity * x.Product.Price);
                    vm.CartId = selected.Select(x => x.Id).ToArray();
                    vm.PaymentType = "Cash on Delivery";
                    vm.CheckedOut += Vm_CheckedOut;
                });

                await Task.CompletedTask;
                //var request = new CheckoutRequest
                //{
                //    CartIds = selected.Select(x => x.Id).ToArray(),
                //    PaymentType = Shared.Enums.PaymentType.Paypal,
                //};
                //var response = await _httpClient.PostAsync<bool>("Cart/Checkout",request);

                //if (response.Succeeded)
                //{
                //    GetCarts();
                //    Toast.Make(response.Message);
                //}
                //else
                //{
                //    Toast.Make(response.Message);
                //}
            }
        }

        private void Vm_CheckedOut(int[] cartIds)
        {
            var tmpcarts = this.Carts.ToList();
            tmpcarts.RemoveAll(x => cartIds.Contains(x.Id));

            this.Carts = tmpcarts.ToObservableCollection();
            if (this.Carts.Count == 0)
            {
                this.IsEmptyVisible = true;
                this.ThereIsSelected = false;
                this.IsCartItemVisible = false;
            }
            else
            {
                this.IsEmptyVisible = false;
                this.ThereIsSelected = true;
                this.IsCartItemVisible = true;
            }
        }

        public async void GetCarts()
        {
            var response = await _httpClient.GetAsync<List<CartModel>>("Cart/GetCart");
            if (response.Succeeded)
            {
                this.Carts = response.Data.Select(x => new CartItemModel(_httpClient)
                {
                    Id = x.Id,
                    IsSelected = x.IsSelected,
                    Product = new Model.ProductModel
                    {
                        Quantity = x.Product.Quantity,
                        Id = x.Product.Id,
                        CategoryId = x.Product.CategoryId,
                        Image = ImageSource.FromStream(() => new MemoryStream(x.Product.Image)),
                        Price = x.Product.Price,
                        ProductId = x.Product.ProductId,
                        ProductName = x.Product.ProductName,
                        Status = x.Product.Status,
                        UOMId = x.Product.UOMId,
                    },
                    Quantity = x.Quantity,
                }).ToObservableCollection();

                if (this.Carts.Count == 0)
                {
                    this.IsEmptyVisible = true;
                    this.ThereIsSelected = false;
                    this.IsCartItemVisible = false;
                }
                else
                {
                    this.IsEmptyVisible = false;
                    this.ThereIsSelected = true;
                    this.IsCartItemVisible = true;
                }
            }
            else
            {

            }

        }
    }
}
