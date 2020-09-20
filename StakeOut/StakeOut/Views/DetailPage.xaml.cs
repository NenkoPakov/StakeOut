using Xamarin.Forms;

namespace StakeOut
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage(DetailPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}