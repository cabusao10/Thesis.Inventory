using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class VerifyUser : ContentPage
{
    public VerifyUser(VerifyViewModel model)
    {
        this.BindingContext = model;
        InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);

        Shell.SetNavBarIsVisible(this, false);
    }
}