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
using Thesis.Inventory.Shared.DTOs.Order.Response;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using Thesis.Inventory.Shared.Enums;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly UserOrderModel _userOrderModel;
        public ProfileViewModel(HttpClient httpClient, UserOrderModel userorder)
        {
            this._httpClient = httpClient;
            
            LoadData();
        }

        public async void LoadData()
        {
            this.IsLoading = true;
            await this.GetProfile();
            await this.GetOders();

            this.IsLoading = false;
        }
        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task Refresh()
        {
            this.IsRefreshing = false;
            this.IsLoading = true;
            await this.GetOders();
            await this.GetProfile();
            await Task.CompletedTask;
            this.IsLoading = false;
        }
        [ObservableProperty]
        GetUserResponse profile;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        ObservableCollection<UserOrderModel> orders;

        public async Task GetProfile()
        {
            var userid = await SecureStorage.GetAsync("userid");
            var response = await _httpClient.GetAsync<GetUserResponse>($"User/GetUserById?id={userid}");
            if (response.Succeeded)
            {
                this.Profile = response.Data;

            }
            else
            {

            }
        }
        public async Task GetOders()
        {
            var response = await _httpClient.GetAsync<IEnumerable<OrderModel>>("Order/CustomerOrders");
            if (response.Succeeded)
            {
                this.Orders = response.Data.Select(x => new UserOrderModel(_httpClient)
                {
                    Id = x.Id,
                    DateOrdered = x.DateCreated,
                    PaymentMethod = x.PaymentMethod,
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
                    Status = x.Status,

                }).ToObservableCollection();

                if (this.Orders.Count == 0)
                {

                }
                else
                {

                }
            }
            else
            {

            }

        }
    }
}
