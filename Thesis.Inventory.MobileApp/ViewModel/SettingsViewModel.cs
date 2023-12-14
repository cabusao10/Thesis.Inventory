using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Shared.DTOs.Users.Responses;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        public SettingsViewModel(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this.GetDataProfile();
        }

        [ObservableProperty]
        GetUserResponse profile;

        private async void GetDataProfile()
        {
            var userid = await SecureStorage.GetAsync("userid");
            var response = await this._httpClient.GetAsync<GetUserResponse>($"user/GetUserById?id={userid}");

            if (response.Succeeded)
            {
                this.Profile = response.Data;
            }
            else
            {

                await Toast.Make("Failed  to fetch user data.").Show();
            }

        }
        [RelayCommand]
        async Task Update()
        {
            var request = new UpdateUserRequest
            {
                Address = this.Profile.Address,
                Birthdate = this.Profile.Birthdate,
                BusinessName = "",
                ContactNumber = this.Profile.ContactNumber,
                Fullname = this.Profile.Fullname,
                Id = this.Profile.Id,
                ProvinceId = this.Profile.ProvinceId,
                Role = (int)this.Profile.Role,
                ZipCode = this.Profile.ZipCode,
            };

            var response = await this._httpClient.PatchAsync<bool>("user/UpdateUser", request);

            if (response.Succeeded)
            {
                await Toast.Make("Success updating the data.").Show();
            }
            else
            {

                await Toast.Make("Failed updating the data.").Show();
            }
        }

        [RelayCommand]
        async Task Signout()
        {
        }
    }
}
