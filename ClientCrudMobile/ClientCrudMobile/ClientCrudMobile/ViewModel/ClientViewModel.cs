using ClientCrudMobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using ClientCrudMobile.Views;

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






        #region comandos
        public Command editarDireccionSelectedCommand { get; set; }
        public Command borrarDireccionSelectedCommand { get; set; }
        public Command agregarDireccionSelectedCommand { get; set; }
        public Command nuevoClienteCommand { get; set; }
        public Command guardarEditarClienteCommand { get; set; }
        public Command borrarClienteCommand { get; set; }
        #endregion

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
                nuevoClienteCommand = new Command(mtNuevoClient);
                guardarEditarClienteCommand = new Command(mtAgregarEditarCliente);
                borrarClienteCommand = new Command(mtBorrarCliente);

            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("", ex.ToString(), "ok");
            }
        }


        #region metodos
        //Obtener el listado de las peliculas destacadas de la semana, vienen en grupo de paginas manejadas por el parametro
        public async void GetClientsList()
        {
            try
            {
                RequestHandler<List<ClientModel>> response = await App.WebService.GetAsync<List<ClientModel>>($"{App.WebService.baseUrl}clientes/");
                if (response.isSuccessRequest)
                {
                    var Clientes = response.RequestResult;
                    if (Clientes.Count > 0)
                    {
                        ClientList = Clientes;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("", "No se pudo obtener la lista de clientes", "ok");

                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("", "Ocurrio un problema realizando la accion", "ok");

            }
        }

        private string isClientModelOk(ClientModel cm)
        {
            string _return = string.Empty;
            int edad = 0;
            if (string.IsNullOrEmpty(cm.nombre))
            {
                _return = "El campo nombre esta vacio";
            }
            else if (string.IsNullOrEmpty(cm.apellido))
            {
                _return = "El campo apellido esta vacio";
            }
            else if (!int.TryParse(cm.edad, out edad) && edad <= 0)
            {
                _return = "Favor ponga una edad valida";
            }
            else if (cm.direcciones == null || cm.direcciones.Count == 0)
            {
                _return = "Debe agregar al menos una direccion";
            }
            return _return;
        }

        #endregion




        #region metodos de commandos
        protected async void mtNuevoClient()
        {
            selectedClient = new ClientModel();
            await App.Current.MainPage.Navigation.PushAsync(new ClientDetail());
        }


        protected async void mtAgregarEditarCliente()
        {
            try
            {
                string messagge = isClientModelOk(SelectedClient);
                if (!string.IsNullOrEmpty(messagge))
                {
                    await App.Current.MainPage.DisplayAlert("", messagge, "ok");
                    return;
                }


                var instanceClientList = ClientList.ToList();
                if (selectedClient.id == 0)
                {
                    string json = JsonConvert.SerializeObject(selectedClient);
                    var response = await App.WebService.PostAsync<ClientModel>($"{App.WebService.baseUrl}clientes/", json);
                    if (response.isSuccessRequest)
                    {
                        var result = response.RequestResult;
                        if (result != null)
                        {
                            selectedClient = result;
                            instanceClientList.Add(selectedClient);
                            ClientList = instanceClientList;
                            await App.Current.MainPage.Navigation.PopAsync();
                            await App.Current.MainPage.DisplayAlert("", "Se guardo correctamente", "ok");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("", "No se pudo guardar el registro", "ok");

                    }
                }
                else
                {
                    string json = JsonConvert.SerializeObject(selectedClient);
                    var response = await App.WebService.PutAsync<ClientModel>($"{App.WebService.baseUrl}clientes/{selectedClient.id}", json);
                    if (response.isSuccessRequest)
                    {
                        var result = response.RequestResult;
                        if (result != null)
                        {

                            int index = instanceClientList.FindIndex(x => x.id == result.id);
                            instanceClientList[index] = result;
                            ClientList = instanceClientList;
                            await App.Current.MainPage.Navigation.PopAsync();
                            await App.Current.MainPage.DisplayAlert("", "Se actualizo correctamente", "ok");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("", "No se pudo guardar el registro", "ok");

                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("", "Ocurrio un problema realizando la accion", "ok");

            }

        }

        protected async void mtBorrarCliente()
        {
            try
            {
                if (selectedClient.id > 0)
                {
                    bool confirmDelete = await App.Current.MainPage.DisplayAlert("", "Esta seguro de querer borrar este cliente?", "Si", "No");
                    if (confirmDelete)
                    {
                        var response = await App.WebService.DeleteAsync<ClientModel>($"{App.WebService.baseUrl}clientes/{selectedClient.id}");
                        if (response.isSuccessRequest)
                        {
                            ClientList = ClientList.Where(x => x.id != selectedClient.id).ToList();
                            OnPropertyChanged();
                            await App.Current.MainPage.Navigation.PopAsync();
                            await App.Current.MainPage.DisplayAlert("", "Cliente eliminado correctamente", "ok");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("", "No se pudo borrar el registro", "ok");

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("", "Ocurrio un problema realizando la accion", "ok");

            }
        }

        protected async void mtAgregarDireccion()
        {
            try
            {
                var instanceCopy = selectedClient.direcciones;
                if (instanceCopy == null)
                {
                    instanceCopy = new List<string>();
                }
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
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("", "Ocurrio un problema realizando la accion", "ok");

            }
        }

        protected async void mtEditarDireccion(string dir)
        {
            try
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
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("", "Ocurrio un problema realizando la accion", "ok");

            }
        }

        protected void mtBorrarDireccion(string dir)
        {
            selectedClient.direcciones = selectedClient.direcciones.Where(x => x != dir).ToList();

        }

        #endregion



    }
}
