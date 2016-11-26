using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GoodVibrations.Web.Models;
using GoodVibrations.Web.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    public class RecordingStatusCallbackController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromQuery(Name = "token")] string token,
            [FromForm(Name = "RecordingUrl")] string recordingUrl,
            [FromForm(Name = "RecordingSid")] string recordingSid,
            [FromForm(Name = "From")] string from,
            [FromForm(Name = "RecordingDuration")] string recordingDuration,
            [FromForm(Name = "CallStatus")] string callStatus)
        {
            //make speech to text possible
            //new CognitiveService().SpeechToText(recordingUrl);

            return Ok();
        }

        // POST api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
