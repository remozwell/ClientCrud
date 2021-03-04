using ClientCrudMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ClientCrudMobile.ViewModel
{
    public class MainViewModel:BaseViewModel
    {

        public ClientViewModel ClientVM { get; private set; }

        public MainViewModel()
        {

            ClientVM = new ClientViewModel();
        }

        
    }
}
