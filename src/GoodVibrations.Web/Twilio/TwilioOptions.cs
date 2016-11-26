using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodVibrations.Web.Twilio
{
    public class TwilioOptions
    {
        public string AccountSid { get; set; }

        public string AuthToken { get; set; }

        public string FromPhoneNumber  { get; set; }

        public string CallVoice { get; set; }

        public string CallLanguage { get; set; }
        
    }
}
