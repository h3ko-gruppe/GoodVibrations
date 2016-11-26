using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.Web.Twilio;
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

        public PhoneCallBackController(IOptions<TwilioOptions> twilioSettings)
        {
            _twilioSettings = twilioSettings;
        }
        
        [HttpPost(Name = GetPhoneCallbackRoute)]
        public async Task<IActionResult> Post([FromQuery]string token)
        {

            var message = "GoodVibrations ruft dich an, voll Geil!";
            var actionUrl = "http://goodvibrations-app.azurewebsites.net/api/recordingstatuscallback?token=ijlsdfnajdfgnfg";
            var twilioCallbackXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                                    "<Response>\r\n" +
                                    $"    <Say voice=\"{_twilioSettings.Value.CallVoice}\" language=\"{_twilioSettings.Value.CallLanguage}\">{message}</Say>\r\n" +
                                    $"    <Record action=\"{actionUrl}\" method=\"POST\" />\r\n" +
                                    "</Response>";

            return Content(twilioCallbackXml, "application/xml");

        }

        
    }
}
