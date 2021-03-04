using System;
using System.Collections.Generic;
using System.Text;

namespace ClientCrudMobile.Model
{
    public class RequestHandler<T>
    {
        public bool isSuccessRequest { get; set; }
        public T RequestResult { get; set; }
    }
}
