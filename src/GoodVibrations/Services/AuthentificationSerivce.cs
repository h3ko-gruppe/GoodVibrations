using System;
using System.Threading.Tasks;
using GoodVibrations.Core.Models;
using GoodVibrations.Interfaces.Services;

namespace GoodVibrations.Services
{
	public class AuthentificationSerivce : IAuthentificationSerivce
	{
			
        public async Task<bool> Login(string username, string password)
		{
            await Task.Delay (1000);
            return true;
		}

        public async Task<bool> Register (string username, string password)
        {
            await Task.Delay (1000);
            return true;
        }

        public string Username {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }
	}
}
