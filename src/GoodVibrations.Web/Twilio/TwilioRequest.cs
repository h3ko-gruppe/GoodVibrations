using System;
using Newtonsoft.Json;

namespace GoodVibrations.Web.Twilio
{
    public class TwilioRequest
    {

        public TwilioRequest(string accountSid, string authToken, string from, string to, string url)
        {
            AccountSid = accountSid;
            From = from;
            To = to;
            Url = url;
            AuthToken = authToken;
        }

        public string AccountSid { get; set; }

        public string To { get; set; }

        public string From { get; set; }

        public string Url { get; set; }
    
        public string AuthToken { get; set; }
    }
}
