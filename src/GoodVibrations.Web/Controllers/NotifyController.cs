using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.Web.BasicAuthentication;
using GoodVibrations.Web.Models;
using GoodVibrations.Web.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    public class NotifyController : ApiHubController<NotifyHub>
    {
        public NotifyController(IConnectionManager signalRConnectionManager) : base(signalRConnectionManager)
        {
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]NotifyRequest notifyRequest)
        {
            Clients.All.Notify(notifyRequest.EventId);
            return Ok($"notification successfull: {notifyRequest.EventId}");
        }

       
    }
}
