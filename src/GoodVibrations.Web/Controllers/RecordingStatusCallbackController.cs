using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GoodVibrations.Web.Data;
using GoodVibrations.Web.Extensions;
using GoodVibrations.Web.Models;
using GoodVibrations.Web.Services;
using GoodVibrations.Web.SmtpEmail;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodVibrations.Web.Controllers
{
    [Route("api/[controller]")]
    public class RecordingStatusCallbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SmtpEmailOptions _smtpEmailOptions;
        private readonly SmtpEmailService _emailService;
         public RecordingStatusCallbackController(IOptions<SmtpEmailOptions> smtpEmailOptions, ApplicationDbContext context)
         {
             _context = context;
             _smtpEmailOptions = smtpEmailOptions.Value;
             _emailService = new SmtpEmailService(smtpEmailOptions.Value);
         }

        public const string GetRecordingStatusCallbackRoute = "GetRecordingStatusCallbackRoute";
        // POST api/values
        [HttpPost(Name = GetRecordingStatusCallbackRoute)]
        public async Task<IActionResult> Post([FromQuery(Name = "token")] string token,
            [FromForm(Name = "RecordingUrl")] string recordingUrl,
            [FromForm(Name = "RecordingSid")] string recordingSid,
            [FromForm(Name = "From")] string from,
            [FromForm(Name = "RecordingDuration")] string recordingDuration,
            [FromForm(Name = "CallStatus")] string callStatus)
        {

            var userEmail = await GetEmailByMessageToken(token);
            if (!string.IsNullOrEmpty(userEmail))
            {
                var fromEmail = new MailboxAddress[] { new MailboxAddress(_smtpEmailOptions.DefaultFrom) };
                const string subject = "This is the Good Vibrations Phone Call Summary";
                var body =
                    $"Hi,\r\nThis is an automatic email summary.\r\nCall Status:{callStatus}\r\nRecording Duration:{recordingDuration}\r\nMp3 Download Url:{recordingUrl}\r\n\r\n\r\nImagine we would have time to send this to a speech to text service! You would be able to read the answer of the person you where calling by text!";
                var toEmail = new MailboxAddress[] {new MailboxAddress(userEmail)};
                await _emailService.SendMail(fromEmail, subject, body, toEmail).ConfigureAwait(false);
                
                return Ok();
            }

            return BadRequest($"The token {token} does not exist");
        }

        private async Task<string> GetEmailByMessageToken(string token)
        {
            var manager = HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
            if (manager != null) { 

                var phoneCall = _context.PhoneCalls.FirstOrDefault(x => x.Token == token);
                if (phoneCall != null)
                {
                    var user = await manager.FindByIdAsync(phoneCall.UserId);
                    if (user != null)
                    {
                        return user.Email;
                    }
                }
            }

            return null;
        }


        // POST api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
