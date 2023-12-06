using Thesis.Inventory.MobileApp.Components;
using Thesis.Inventory.MobileApp.ViewModel;
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class ShopPage : ContentPage
{
    public GetProductResponse sample { get; set; }
    public ShopPage(ShopPageViewModel view)
    {
        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
        BindingContext = view;
    }

    public ShopPage()
    {
        InitializeComponent();
    }
}