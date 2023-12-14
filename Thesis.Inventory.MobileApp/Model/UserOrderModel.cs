using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.Shared.DTOs.Order.Request;
using Thesis.Inventory.Shared.Enums;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.MobileApp.Model
{
    public partial class UserOrderModel : ObservableObject
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public PaymentType PaymentMethod { get; set; }

        [ObservableProperty]
        OrderStatusType status;
        public DateTime DateOrdered { get; set; }
        public ProductModel Product { get; set; }


        private readonly HttpClient _httpClient;
        public UserOrderModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [RelayCommand]
        async Task CancelOrder()
        {
            var request = new ChangeOrderStatusRequest
            {
                NewStatus = OrderStatusType.Cancelled,
                OrderId = this.Id,
            };
            var response = await _httpClient.PatchAsync<bool>("Order/ChangeStatus", request);

            if(response.Succeeded)
            {
                this.Status = OrderStatusType.Cancelled;
                
            }
            else
            {

            }
        }
    }
}
