
using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly MainPage _mainpage;
    public SettingsPage(SettingsViewModel model, MainPage mainpage)
    {
        this.BindingContext = model;
        this._mainpage = mainpage;
        InitializeComponent();
    }
    public SettingsPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await AppShell.Current.GoToAsync(nameof(MainPage));
    }
}