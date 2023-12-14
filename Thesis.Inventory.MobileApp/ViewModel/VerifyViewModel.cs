using CommunityToolkit.Maui.Alerts;
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

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class VerifyViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        public VerifyViewModel(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        [ObservableProperty]
        string verifcationCode;

        [RelayCommand]
        async Task VerifyUser()
        {
            var request = new VerifyUserRequest
            {
                OTP = this.VerifcationCode,
            };

            var response = await _httpClient.PostAsync<bool>("User/Verify", request);

            if (response.Succeeded)
            {
                if (response.Data)
                {
                    var toast = Toast.Make(response.Message, CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show();
                    await AppShell.Current.GoToAsync(nameof(HomePage));
                }
                else
                {
                    var toast = Toast.Make(response.Message, CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show();
                }
            }
            else
            {
                var toast = Toast.Make(response.Message, CommunityToolkit.Maui.Core.ToastDuration.Short);
                await toast.Show();
            }
        }
    }
}
