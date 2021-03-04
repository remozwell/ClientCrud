using System;
using System.Collections.Generic;
using System.Text;

namespace ClientCrudMobile.Model
{
    public class ClientModel : BaseModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string edad { get; set; }
        private List<string> _direcciones { get; set; }
        public List<string> direcciones
        {
            get
            {
                return _direcciones;
            }
            set
            {
                _direcciones = value;
                OnPropertyChanged();
            }
        }
    }
}
