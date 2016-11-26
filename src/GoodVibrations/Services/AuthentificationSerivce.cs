using System;
using System.Threading.Tasks;
using GoodVibrations.ApiClient;
using GoodVibrations.Core.Models;
using GoodVibrations.Extensions;
using GoodVibrations.Interfaces.Services;

namespace GoodVibrations.Services
{
	public class AuthentificationSerivce : IAuthentificationSerivce
	{

        public async Task<bool> Login(string username, string password)
		{
            var testToken = CreateBasicAuthToken (username, password);
            var client = new RestClient (testToken);
            var isSuccessful = await client.Login ();

            Username = isSuccessful ? username : string.Empty;
            password = isSuccessful ? password : string.Empty;

            return isSuccessful;
		}

        public async Task<bool> CreateAccount (string username, string password)
        {
            var client = new RestClient ();
            var isSuccessful = await client.CreateAccount (username, password);

            Username = isSuccessful ? username : string.Empty;
            password = isSuccessful ? password : string.Empty;

            return isSuccessful;
        }

        public string Username {
            get;
            private set;
        }

        public string Password {
            get;
            private set;
        }

        private string CreateBasicAuthToken (string username, string password)
        {
            var token = $"{username}:{password}".Base64Encode();
            return token;
        }

        public string BasicAuthToken {
            get { 
                return CreateBasicAuthToken (Username, Password);
            }          
        }
	}
}
