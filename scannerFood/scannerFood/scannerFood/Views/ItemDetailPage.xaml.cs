using scannerFood.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace scannerFood.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}