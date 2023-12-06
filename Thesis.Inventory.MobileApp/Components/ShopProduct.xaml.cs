using CommunityToolkit.Mvvm.ComponentModel;
using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Components;

public partial class ShopProduct : ContentView
{
    public ShopProductViewModel ViewModel { get; set; }
    public ShopProduct(ShopProductViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        ViewModel = model;
    }
    public ShopProduct()
    {
        InitializeComponent();
    }


}