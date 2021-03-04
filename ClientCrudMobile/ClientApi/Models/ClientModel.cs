using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientApi.Models
{
    public class ClientModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string edad { get; set; }
        public List<string> direcciones { get; set; }
      
    }
}