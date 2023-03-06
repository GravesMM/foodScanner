using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace scannerFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectionPage : ContentPage
    {
        public SelectionPage()
        {
            InitializeComponent();

            EggCheckbox.IsChecked = Preferences.Get("EggCheckbox", false);
            CowCheckbox.IsChecked = Preferences.Get("CowCheckbox", false);
            SoyCheckbox.IsChecked = Preferences.Get("SoyCheckbox", false);
            WheatCheckbox.IsChecked = Preferences.Get("WheatCheckbox", false);
            MolluscanCheckbox.IsChecked = Preferences.Get("MolluscanCheckbox", false);
            CrustaceanCheckbox.IsChecked = Preferences.Get("CrustaceanCheckbox", false);
            FishCheckbox.IsChecked = Preferences.Get("FishCheckbox", false);
            PeanutCheckbox.IsChecked = Preferences.Get("PeanutCheckbox", false);
            TreenutCheckbox.IsChecked = Preferences.Get("TreenutCheckbox", false);
        }
    }
}