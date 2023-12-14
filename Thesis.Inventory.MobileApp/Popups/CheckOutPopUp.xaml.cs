using CommunityToolkit.Maui.Views;
using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Popups;

public partial class CheckOutPopUp : Popup
{
	public CheckOutPopUp(CheckOutViewModel model)
	{
		this.BindingContext = model;
		InitializeComponent();

        model.CheckedOut += Model_CheckedOut;
	}

    private async void Model_CheckedOut(int[] ids)
    {
       await this.CloseAsync();
    }
}