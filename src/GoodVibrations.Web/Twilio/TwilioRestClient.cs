using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GoodVibrations.Web.Extensions;

namespace GoodVibrations.Web.Twilio
{
    public class TwilioRestClient
    {
        private const string ApiUrl = "https://api.twilio.com/2010-04-01/Accounts/{TwilioAccountSid}/Calls.json";
        public Task<HttpResponseMessage> Post(TwilioRequest request, TwilioOptions options)
        {
            var authToken = $"{request.AccountSid}:{request.AuthToken}".Base64Encode();
            var formData = new Dictionary<string, string>
            {
                {"From", request.From},
                {"To", request.To},
                {"Url", request.Url}
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",authToken);
            return client.PostAsync(ApiUrl.Replace("{TwilioAccountSid}", options.AccountSid), new FormUrlEncodedContent(formData));
        }
    }
}
