using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class CartPage : ContentPage
{
	public CartPage(CartViewModel viewModel)
	{
        this.BindingContext = viewModel;
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    private void SwipeItem_Invoked(object sender, EventArgs e)
    {
        var model = (CartViewModel)BindingContext;
        model.GetCarts();
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var model = (CartViewModel)BindingContext;
        model.ThereIsSelected = model.Carts.Any(x => x.IsSelected);
    }
}