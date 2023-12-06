using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.MobileApp.Pages;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Shared.DTOs.Users.Responses;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        private readonly HttpClient _httpClient;
        public MainViewModel(HttpClient httpClient)
        {
            username = string.Empty;
            password = string.Empty;
            _httpClient = httpClient;
        }

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task GoToRegisterPage()
        {
            await AppShell.Current.GoToAsync(nameof(RegisterPage));
        }
        [RelayCommand]
        async Task Login()
        {
            ToastDuration duration = ToastDuration.Short;

            // login process
            var request = new UserLoginRequest
            {
                Username = this.Username,
                Password = this.Password,
            };
            var response = await _httpClient.PostAsync<UserLoginResponse>("user/mobilelogin", request);
            if(response == null)
            {

                var toast = Toast.Make("There is an error with backend!", duration, 14);

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                await toast.Show(cancellationTokenSource.Token);
            }
            if (response.Succeeded)
            {
                await SecureStorage.Default.SetAsync("username", this.Username);
                await SecureStorage.Default.SetAsync("token", response.Data.BearerToken);
                await SecureStorage.Default.SetAsync("userid", response.Data.Id.ToString());

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", response.Data.BearerToken);

                await AppShell.Current.GoToAsync(nameof(HomePage));

            }
            else
            {
                var toast = Toast.Make(response.Message, duration, 14);

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                await toast.Show(cancellationTokenSource.Token);
            }

        }
    }
}
