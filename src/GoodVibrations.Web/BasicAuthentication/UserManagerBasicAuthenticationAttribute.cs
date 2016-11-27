using System.Net;
using GoodVibrations.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodVibrations.Web.BasicAuthentication
{
    public class UserManagerBasicAuthenticationAttribute : BasicAuthenticationBaseAttribute
    {

        protected override bool Authorize(ActionExecutingContext actionExecutingContext, NetworkCredential credential)
        {
            var isValid = false;
            var manager = actionExecutingContext.HttpContext.RequestServices.GetService(typeof (UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
            if (manager != null)
            {
                var user = manager.FindByNameAsync(credential.UserName).Result;
                isValid = manager.CheckPasswordAsync(user, credential.Password).Result;
            }

            return isValid;
        }
    }
}
