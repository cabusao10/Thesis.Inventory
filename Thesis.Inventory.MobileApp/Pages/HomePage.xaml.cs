using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(ProfileViewModel model)
	{
        this.BindingContext = model;
		InitializeComponent();
        Shell.SetTabBarIsVisible(this, true);
        
        Shell.SetNavBarIsVisible(this, false);
    }
}