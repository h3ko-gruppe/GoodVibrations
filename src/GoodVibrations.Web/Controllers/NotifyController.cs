using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.Web.BasicAuthentication;
using GoodVibrations.Web.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    public class NotifyController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]NotifyRequest notifyRequest)
        {
            return Ok($"notification successfull: {notifyRequest.EventId}");
        }
    }
}
