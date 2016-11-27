using System;
using Newtonsoft.Json;

namespace GoodVibrations.ApiClient
{
    public class RegistrationRequest
    {
        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("password")]
        public string Password { get; set; }
    }
}
