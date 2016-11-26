using System;
using System.Threading.Tasks;
using GoodVibrations.Models;

namespace GoodVibrations.Interfaces.Services
{
	public interface IPhoneCallService
	{
        Task<bool> StartCall (PhoneCall call);
    }
}
