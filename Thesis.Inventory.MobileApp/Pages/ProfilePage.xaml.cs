using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel model)
	{
		this.BindingContext = model;

		InitializeComponent();
	}
}