using System;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;

namespace GoodVibrations.Services
{
	public class PhoneCallService : IPhoneCallService
	{
		public async Task<bool> StartCall (PhoneCall call)
        {
            await Task.Delay (1000);
            return true;
        }
	}
}
