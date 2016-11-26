using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using Refit;

namespace GoodVibrations.ApiClient
{
    public class RestClient
    {
       
        private readonly IRestApi _api;

        public RestClient (HttpClient httpClient, string basicToken)
        {
            _api = RestService.For<IRestApi> (httpClient);
            AuthorizationHeaderValue = $"Basic {basicToken}";
        }

        public RestClient (string hostUrl, string basicToken)
        {
            _api = RestService.For<IRestApi> (hostUrl);
            AuthorizationHeaderValue = $"Basic {basicToken}";
        }

        protected string AuthorizationHeaderValue { get; set; }

        public async Task<bool> Login (string username, string password)
        {
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
            var result = await _api.MakePhoneCall (AuthorizationHeaderValue);
            return result.IsSuccessStatusCode;
        }
    }
}
