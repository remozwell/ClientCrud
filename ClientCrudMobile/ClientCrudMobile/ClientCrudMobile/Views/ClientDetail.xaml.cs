using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientCrudMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientDetail : ContentPage
    {
        public ClientDetail()
        {
            BindingContext = App.MainViewModel.ClientVM;
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}