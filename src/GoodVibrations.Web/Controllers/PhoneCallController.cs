using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.Web.Data;
using GoodVibrations.Web.Models;
using GoodVibrations.Web.Twilio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Robin.Web.Extensions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    public class PhoneCallController : Controller
    {

        private readonly IOptions<TwilioOptions> _twilioSettings;
        private readonly ApplicationDbContext _context;

        public PhoneCallController(IOptions<TwilioOptions> twilioSettings, ApplicationDbContext context)
        {
            _twilioSettings = twilioSettings;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var token = "wesrbhjknlm";
            //var callbackUrl = Url.RouteUrl(PhoneCallBackController.GetPhoneCallbackRoute, new {token}, Request.Scheme, Request.Host.ToUriComponent());
            var callbackUrl = "http://goodvibrations-app.azurewebsites.net/api/phonecallback";
            var toPhoneNumber = "+4915208982338";
            var twilioClient = new TwilioRestClient();
            var request = new TwilioRequest(_twilioSettings.Value.AccountSid, _twilioSettings.Value.AuthToken, _twilioSettings.Value.FromPhoneNumber, toPhoneNumber, callbackUrl);
            var result = await twilioClient.Post(request, _twilioSettings.Value);

            if (result.IsSuccessStatusCode)
                return Ok(await result.Content.ReadAsStringAsync());
            else
                return Ok(result.StatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PhoneCallRequest req)
        {
            var token = $"{Guid.NewGuid().ToString("N")+Guid.NewGuid().ToString("N")}";
            var currentUser = await User.GetUser(HttpContext);
            var call = new PhoneCall
            {
                Token = token,
                User = currentUser,
                UserId = currentUser.Id,
                FromPhoneNumber = req.FromNumber,
                CreatedAt = DateTime.Now,
                CurrentLocation = req.CurrentLocation,
                Message = req.Message,
                ToPhoneNumber = req.ToNumber
            };

            _context.PhoneCalls.Add(call);
            await _context.SaveChangesAsync();
            
            var callbackUrl = Url.RouteUrl(PhoneCallBackController.GetPhoneCallbackRoute, new {token}, Request.Scheme, Request.Host.ToUriComponent());
            //var callbackUrl = "http://goodvibrations-app.azurewebsites.net/api/phonecallback";
            var toPhoneNumber = req.ToNumber;
            var twilioClient = new TwilioRestClient();
            var request = new TwilioRequest(_twilioSettings.Value.AccountSid, _twilioSettings.Value.AuthToken, _twilioSettings.Value.FromPhoneNumber, toPhoneNumber, callbackUrl);
            var result = await twilioClient.Post(request, _twilioSettings.Value);

            if (result.IsSuccessStatusCode)
                return Ok(await result.Content.ReadAsStringAsync());
            else
                return Ok(result.StatusCode);
        }
    }
}
