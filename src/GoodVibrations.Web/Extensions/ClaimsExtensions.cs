using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodVibrations.Web.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Robin.Web.Models;

namespace Robin.Web.Extensions
{
    public static class ClaimsExtensions
    {
        public static async Task<ApplicationUser> GetUser(this ClaimsPrincipal identity, HttpContext context)
        {
            var usermanager = context.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            ApplicationUser user = null;
            if (usermanager != null)
            {
                user = await usermanager.FindByNameAsync(identity.Identity.Name);
            }
            return user;
        }
    }
}
