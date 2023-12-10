using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Shared.Enums;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class RegisterViewModel : ObservableObject
    {

        private ToastDuration duration = ToastDuration.Short;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private HttpClient _httpClient;
        private IEnumerable<GetProvincesResponse> _provinces_data;
        public RegisterViewModel(HttpClient httpClient)
        {

            _httpClient = httpClient;
            GetProvinces();
            this.BirthDate = DateTime.Now;
        }

        [ObservableProperty]
        List<string> provinces;

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string confirmpassword;

        [ObservableProperty]
        string fullname;

        [ObservableProperty]
        string contactNumber;

        [ObservableProperty]
        string address;

        [ObservableProperty]
        string zipCode;

        [ObservableProperty]
        DateTime birthDate;

        [ObservableProperty]
        GenderType gender;

        [ObservableProperty]
        UserRoleType role;

        [ObservableProperty]
        UserStatusType status;

        [ObservableProperty]
        string businessname;

        [ObservableProperty]
        string email;

        [ObservableProperty]
        int provinceId;

        [RelayCommand]
        async Task Register()
        {
            var province = Convert.ToInt32(_provinces_data.ToArray()[this.ProvinceId].Key);
            var request = new UserRegisterRequest
            {
                Username = this.Username,
                Password = this.Password,
                ConfirmPassword = this.Confirmpassword,
                Fullname = this.Fullname,
                Email = this.Email,
                Address = this.Address,
                Birthdate = this.BirthDate,
                ContactNumber = this.ContactNumber,
                Gender = this.Gender,
                ProvinceId = province,
                ZipCode = this.ZipCode,
                Role = UserRoleType.Consumer,
            };

            var response = await _httpClient.PostAsync<bool>("user/Register", request);

            if (response.Succeeded)
            {
                var toast = Toast.Make("Verify your email!", duration, 14);
                await AppShell.Current.GoToAsync($"{nameof(MainPage)}");
                await toast.Show(cancellationTokenSource.Token);
            }
            else
            {
                Toast.Make(response.Message);
            }
        }

        async void GetProvinces()
        {
            var response = await _httpClient.GetAsync<IEnumerable<GetProvincesResponse>>("user/GetProvinces");
            _provinces_data = response.Data;
            this.Provinces = response.Data.Select(x => x.Text).ToList();

        }
        [RelayCommand]
        async Task BackToLogin()
        {
            await AppShell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
