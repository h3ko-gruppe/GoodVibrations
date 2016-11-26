using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GoodVibrations.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationRequest reg)
        {
            var manager = HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
            if (manager != null)
            {
                var exists = await manager.FindByNameAsync(reg.Email) != null;
                if (!exists)
                {
                    var user = new ApplicationUser
                    {
                        Email = reg.Email,
                        UserName = reg.Email,
                    };
                    await manager.CreateAsync(user, reg.Password);
                    return Ok();
                }
                else
                {
                    return BadRequest("This user already exists");
                }
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "Usermanager is not availible. This should never happen!");
        }
    }
}
