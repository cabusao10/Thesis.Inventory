using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }
    public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = viewModel;
    }
}