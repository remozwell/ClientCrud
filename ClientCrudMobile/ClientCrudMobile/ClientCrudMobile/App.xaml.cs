using ClientCrudMobile.Helpers;
using ClientCrudMobile.ViewModel;
using ClientCrudMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientCrudMobile
{
    public partial class App : Application
    {
        public static MainViewModel MainViewModel { get; private set; }

        public static WebService WebService { get; private set; }

        static App()
        {
            WebService = new WebService();
            MainViewModel = new MainViewModel();
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Clientes());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
