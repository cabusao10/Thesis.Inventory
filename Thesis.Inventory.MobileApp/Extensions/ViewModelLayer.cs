using CommunityToolkit.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Components;
using Thesis.Inventory.MobileApp.Model;
using Thesis.Inventory.MobileApp.Pages;
using Thesis.Inventory.MobileApp.Popups;
using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Extensions
{
    public static class ViewModelLayer
    {
        public static void AddViewModelLayer(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ShopPageViewModel>();
            services.AddSingleton<ShopProductViewModel>();
            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<CartViewModel>();
            services.AddTransient<AddToCartViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<UserOrderModel>();
            services.AddTransient<VerifyViewModel>();
            services.AddTransient<ChatViewModel>();

            services.AddSingleton<ShopPage>();
            services.AddSingleton<MainPage>();
            services.AddSingleton<ShopProduct>();
            services.AddSingleton<CartPage>();
            services.AddSingleton<ProfilePage>();
            services.AddSingleton<RegisterPage>();
            services.AddSingleton<HomePage>();
            services.AddSingleton<VerifyUser>();
            services.AddSingleton<ChatPage>();

            services.AddTransientPopup<AddToCartPopUp, AddToCartViewModel>();
            services.AddTransientPopup<CheckOutPopUp, CheckOutViewModel>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
