using System;
using System.Threading.Tasks;
using GoodVibrations.Models;

namespace GoodVibrations.Interfaces.Services
{
	public interface IPhoneCallService
	{
	    Task<bool> StartCall(string message, string toPhoneNumber, string fromPhoneNumber = null,
	        string currentLocation = null);
	}
}
