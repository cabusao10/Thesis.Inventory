using Thesis.Inventory.MobileApp.ViewModel;

namespace Thesis.Inventory.MobileApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
            BindingContext = mainViewModel;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ((MainViewModel)BindingContext).IsPassword = !e.Value;
        }
    }
}