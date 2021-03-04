using ClientCrudMobile.Model;
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
    public partial class Clientes : ContentPage
    {
        public Clientes()
        {
            InitializeComponent();

            BindingContext = App.MainViewModel;
        }

        private void Client_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ClientModel Client = (ClientModel)e.Item;
            Navigation.PushAsync(new ClientDetail(), true);
        }
    }
}