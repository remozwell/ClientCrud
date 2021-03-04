using ClientCrudMobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace ClientCrudMobile.ViewModel
{
    public class ClientViewModel : BaseViewModel
    {
        private List<ClientModel> _ClientList { get; set; }
        public List<ClientModel> ClientList
        {
            get
            {
                return _ClientList;
            }
            set
            {
                _ClientList = value;
                OnPropertyChanged();
            }
        }


        ClientModel selectedClient;
        public ClientModel SelectedClient
        {
            get
            {
                return selectedClient;
            }
            set
            {
                SetPropertyValue(ref selectedClient, value);
            }
        }







        public Command editarDireccionSelectedCommand { get; set; }
        public Command borrarDireccionSelectedCommand { get; set; }
        public Command agregarDireccionSelectedCommand { get; set; }
        public Command guardarEditarClienteCommand { get; set; }

        public ClientViewModel()
        {
            try
            {
                //Traer la data
                ClientList = new List<ClientModel>();
                GetClientsList();


                //Comandos
                editarDireccionSelectedCommand = new Command<string>((direccion) => mtEditarDireccion(direccion));
                borrarDireccionSelectedCommand = new Command<string>((direccion) => mtBorrarDireccion(direccion));
                agregarDireccionSelectedCommand = new Command(mtAgregarDireccion);
                guardarEditarClienteCommand = new Command(mtAgregarEditarCliente);

            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("", ex.ToString(), "ok");
            }
        }


        //Obtener el listado de las peliculas destacadas de la semana, vienen en grupo de paginas manejadas por el parametro
        public async void GetClientsList()
        {
            List<ClientModel> Clientes = await App.WebService.GetAsync<List<ClientModel>>($"{App.WebService.baseUrl}clientes/");
            if (Clientes.Count > 0)
            {
                ClientList = Clientes;
            }

        }





        #region commandos
        protected async void mtAgregarEditarCliente()
        {
            if (selectedClient.id == 0)
            {
                string json = JsonConvert.SerializeObject(selectedClient);
                var response = await App.WebService.PostAsync<ClientModel>($"{App.WebService.baseUrl}clientes/", json);
                if (response != null)
                {
                    selectedClient.id = response.id;
                    ClientList.Add(selectedClient);
                    OnPropertyChanged();
                    await App.Current.MainPage.DisplayAlert("", "Se guardo correctamente", "ok");
                }
            }
            else
            {
                string json = JsonConvert.SerializeObject(selectedClient);
                var response = await App.WebService.PuttAsync<ClientModel>($"{App.WebService.baseUrl}clientes/", json);
                if (response != null)
                {
                    int index = ClientList.FindIndex(x => x.id == response.id);
                    ClientList[index] = response;
                    OnPropertyChanged();
                    await App.Current.MainPage.DisplayAlert("", "Se actualizo correctamente", "ok");
                }
            }


        }


        protected async void mtAgregarDireccion()
        {
            var instanceCopy = selectedClient.direcciones.ToList();
            var prompt = await App.Current.MainPage.DisplayPromptAsync("", "Escriba la dirección");
            if (prompt != null)
            {
                if (prompt == "")
                {
                    await App.Current.MainPage.DisplayAlert("", "La direccion puesta esta vacia", "Ok");
                    return;
                }

                if (instanceCopy.Any(x => x == prompt))
                {
                    await App.Current.MainPage.DisplayAlert("", "Ya existe esta direción", "Ok");
                    return;
                }
                instanceCopy.Add(prompt);

                selectedClient.direcciones = instanceCopy;
            }
        }

        protected async void mtEditarDireccion(string dir)
        {
            var instanceCopy = selectedClient.direcciones.ToList();
            int index = instanceCopy.IndexOf(dir);
            var prompt = await App.Current.MainPage.DisplayPromptAsync("", "Escriba la dirección", initialValue: dir);
            if (prompt != null)
            {
                if (prompt == "")
                {
                    await App.Current.MainPage.DisplayAlert("", "La direccion puesta esta vacia", "Ok");
                    return;
                }

                if (instanceCopy.Any(x => x == prompt))
                {
                    await App.Current.MainPage.DisplayAlert("", "Ya existe esta direción", "Ok");
                    return;
                }

                instanceCopy[index] = prompt;
                selectedClient.direcciones = instanceCopy;
            }
        }


        protected void mtBorrarDireccion(string dir)
        {
            selectedClient.direcciones = selectedClient.direcciones.Where(x => x != dir).ToList();

        }

        #endregion



    }
}
