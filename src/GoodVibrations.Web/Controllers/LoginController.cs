using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.Web.BasicAuthentication;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    [UserManagerBasicAuthentication]
    public class LoginController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("success");
        }

       
    }
}
