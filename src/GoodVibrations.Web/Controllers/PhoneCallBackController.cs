using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GoodVibrations.Web.Data;
using GoodVibrations.Web.Models;
using GoodVibrations.Web.Twilio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    public class PhoneCallBackController : Controller
    {
        public const string GetPhoneCallbackRoute = "PhoneCallback";
        private readonly IOptions<TwilioOptions> _twilioSettings;
        private readonly ApplicationDbContext _context;

        public PhoneCallBackController(IOptions<TwilioOptions> twilioSettings, ApplicationDbContext context)
        {
            _twilioSettings = twilioSettings;
            _context = context;
        }
        
        [HttpPost(Name = GetPhoneCallbackRoute)]
        public IActionResult Post([FromQuery]string token)
        {
            var call = _context.PhoneCalls.FirstOrDefault(x => x.Token == token);
            if (call != null)
            {
                //var actionUrl = "http://goodvibrations-app.azurewebsites.net/api/recordingstatuscallback?token=ijlsdfnajdfgnfg";
                var actionUrl = Url.RouteUrl(RecordingStatusCallbackController.GetRecordingStatusCallbackRoute, new { token }, Request.Scheme, Request.Host.ToUriComponent());
                var twilioCallbackXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                                        "<Response>\r\n" +
                                        $"    <Say voice=\"{_twilioSettings.Value.CallVoice}\" language=\"{_twilioSettings.Value.CallLanguage}\">{call.Message}</Say>\r\n" +
                                        $"    <Record action=\"{actionUrl}\" method=\"POST\" />\r\n" +
                                        "</Response>";

                return Content(twilioCallbackXml, "application/xml");
            }
            return BadRequest($"The token {token} does not exist!");
        }

        
    }
}
