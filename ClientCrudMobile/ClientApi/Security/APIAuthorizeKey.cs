using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ClientApi.Security
{
    public class APIAuthorizeKey : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }


            base.OnAuthorization(actionContext);
            HandleUnauthorizedRequest(actionContext);

        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, "001-Acceso no autorizado.");
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            bool _return = false;
            try
            {
                if (actionContext.Request.Headers.Authorization.Scheme == "Bearer")
                {
                    string token = actionContext.Request.Headers.Authorization.Parameter;
                    if (!string.IsNullOrEmpty(token))
                    {
                        if (token == ConfigurationManager.AppSettings["PublicTokenKey"].ToString())
                        {
                            _return = true;
                        }
                    }
                }
            }
            catch
            {

            }


            return _return;
        }
    }
}