using Thesis.Inventory.MobileApp.Pages;

namespace Thesis.Inventory.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(VerifyUser), typeof(VerifyUser));
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        }
    }
}