using System;
using System.Net;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodVibrations.Web.BasicAuthentication
{
    public abstract class BasicAuthenticationBaseAttribute : ActionFilterAttribute
    {
        public string BasicRealm { get; set; } = "None";
      
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            
            var req = actionExecutingContext.HttpContext.Request;
            var auth = req.Headers.ContainsKey("Authorization") ?  req.Headers["Authorization"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(auth) && auth.ToLower().StartsWith("basic "))
            {
                NetworkCredential credential;
                try
                {
                    var clearheader = Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                    credential = new NetworkCredential(clearheader[0], clearheader[1]);
                }
                catch (Exception)
                {
                    //something went wrong while parsing the auth header
                    actionExecutingContext.Result = new BadRequestResult();
                    return;
                }

                var isValid = Authorize(actionExecutingContext, credential);
                
                if (!isValid)
                    actionExecutingContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                else
                {
                    actionExecutingContext.HttpContext.User = new GenericPrincipal(new GenericIdentity(credential.UserName), null);
                }
                 
                //in case of valid credentials, do nothing (do not abort operation!)
                return;
            }
            actionExecutingContext.HttpContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", BasicRealm));
            actionExecutingContext.Result = new UnauthorizedResult();
        }

        protected abstract bool Authorize(ActionExecutingContext actionExecutingContext, NetworkCredential credential);
        
    }
}
