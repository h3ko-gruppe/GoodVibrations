using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GoodVibrations.Consts;
using GoodVibrations.Interfaces.Services;
using Refit;

namespace GoodVibrations.ApiClient
{
    public class RestClient
    {
       
        private readonly IRestApi _api;


        public RestClient (string basicToken = null)
        {
            _api = RestService.For<IRestApi> (Constants.RestApi.HostUrl);
            if (!string.IsNullOrEmpty(basicToken))
                AuthorizationHeaderValue = $"Basic {basicToken}";
        }

        protected string AuthorizationHeaderValue { get; set; }

        public async Task<bool> Login ()
        {
            if (string.IsNullOrEmpty (AuthorizationHeaderValue))
                throw new NotSupportedException ("Authentication token required! Please instantiate with valid basic token argument");
            
            var result = await _api.Login (AuthorizationHeaderValue);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> CreateAccount (string username, string password)
        {
            var result = await _api.CreateAccount (username, password);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> MakePhoneCall ()
        {
            if (string.IsNullOrEmpty (AuthorizationHeaderValue))
                throw new NotSupportedException ("Authentication token required! Please instantiate with valid basic token argument");

            var result = await _api.MakePhoneCall (AuthorizationHeaderValue);
            return result.IsSuccessStatusCode;
        }
    }
}
