using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp.Pages;

public partial class ChatPage : ContentPage
{
	public ChatPage(ChatViewModel model)
	{
		this.BindingContext = model;

        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
	}
    public ChatPage()
    {

        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
    }
}