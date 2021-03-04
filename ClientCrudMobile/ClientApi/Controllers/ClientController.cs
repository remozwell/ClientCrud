using ClientApi.Models;
using ClientApi.Security;
using JsonFlatFileDataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ClientApi.Controllers
{

    [APIAuthorizeKey]
    [RoutePrefix("api")]
    public class ClientController : ApiController
    {
        public DataStore store = new DataStore(System.Configuration.ConfigurationManager.AppSettings["ClientDB"].ToString());


        [HttpGet]
        [Route("Clients")]
        public HttpResponseMessage mtGetClientList()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, getClientList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [HttpGet]
        [Route("Clients")]
        public HttpResponseMessage mtGetClient(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, getClient(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("Clients")]
        public HttpResponseMessage mtNewClient(ClientModel client)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, saveClient(client));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("Clients")]
        public HttpResponseMessage mtUpdateClient(ClientModel client)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, updateClient(client));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Route("Clients")]
        public HttpResponseMessage mtDeleteClient(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, deleteClient(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }







        #region metodos
        private List<ClientModel> getClientList()
        {
            var collection = store.GetCollection<ClientModel>().AsQueryable().ToList();
            if (collection == null)
            {
                return new List<ClientModel>();
            }
            else
            {
                return collection;
            }

        }

        private ClientModel getClient(int id)
        {
            var client = store.GetCollection<ClientModel>().AsQueryable().FirstOrDefault(x=> x.id == id);
            if (client != null)
            {
                return client;
            }
            else
            {
                return null;
            }

        }

        private ClientModel saveClient(ClientModel client)
        {
            var collection = store.GetCollection<ClientModel>();
            bool success = collection.InsertOne(client);
            if (success)
            {
                return client;
            }
            else
            {
                throw new Exception(message: "No se pudo guardar el cliente");
            }


        }

        private ClientModel updateClient(ClientModel client)
        {
            var collection = store.GetCollection<ClientModel>();
            var dbItem = collection.AsQueryable().FirstOrDefault(x => x.id == client.id);
            if (dbItem != null)
            {
                collection.UpdateOne(client.id, client);
            }
            else
            {
                throw new Exception(message: "Esta persona no existe en la base de datos");
            }

            return client;

        }


        private bool deleteClient(int id)
        {
            var collection = store.GetCollection<ClientModel>();
            return collection.DeleteOne(x=>x.id == id);
           

        }
        #endregion
    }
}