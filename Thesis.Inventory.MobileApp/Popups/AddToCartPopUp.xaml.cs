using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Popups;

public partial class AddToCartPopUp : Popup
{
    private AddToCartViewModel model;
    public AddToCartPopUp(AddToCartViewModel viewmodel)
    {
        InitializeComponent();
        this.model = viewmodel;
        BindingContext = viewmodel;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var result = await this.model.AddToCart();
        if (result)
        {
            await this.CloseAsync();
            var toast = Toast.Make("Success adding to cart.", CommunityToolkit.Maui.Core.ToastDuration.Short, 14);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}