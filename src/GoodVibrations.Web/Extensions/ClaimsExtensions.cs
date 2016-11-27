using System.Security.Claims;
using System.Threading.Tasks;
using GoodVibrations.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GoodVibrations.Web.Extensions
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
