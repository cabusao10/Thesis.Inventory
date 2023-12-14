using CommunityToolkit.Maui;
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
using Thesis.Inventory.MobileApp.Model;
using Thesis.Inventory.Shared.DTOs.Cart;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class CheckOutViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        public CheckOutViewModel(HttpClient httpClient)
        {

            _httpClient = httpClient;
            this.PaymentMethods = new string[] { "Cash on Delivery" };
        }
        [ObservableProperty]
        double totalPrice;

        [ObservableProperty]
        int[] cartId;

        [ObservableProperty]
        string paymentType;

        [ObservableProperty]
        string[] paymentMethods;

        [RelayCommand]
        async Task CheckOut()
        {
            // check if there is selected
            //Uri uri = new Uri("https://app.nextpay.world/#/pl/66eFe4aSO");
            //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            var request = new CheckoutRequest
            {
                CartIds = this.CartId,
                PaymentType = (PaymentType)this.PaymentMethods.ToList().IndexOf(this.PaymentType),
            };
            var response = await _httpClient.PostAsync<bool>("Cart/Checkout", request);

            if (response.Succeeded)
            {
                var toast = Toast.Make(response.Message, CommunityToolkit.Maui.Core.ToastDuration.Short, 14);

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                await toast.Show(cancellationTokenSource.Token);
            }
            else
            {
                Toast.Make(response.Message);
            }
            this.CheckedOut.Invoke(this.CartId);
        }

        public delegate void DoneCheckingOut(int[] cartId);
        public event DoneCheckingOut CheckedOut;
    }
}
