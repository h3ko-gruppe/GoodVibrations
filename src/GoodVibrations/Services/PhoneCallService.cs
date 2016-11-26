using System;
using System.Threading.Tasks;
using GoodVibrations.ApiClient;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;

namespace GoodVibrations.Services
{
	public class PhoneCallService : IPhoneCallService
	{
	    private readonly IAuthentificationSerivce _authentificationService;

        public PhoneCallService(IAuthentificationSerivce authentificationService)
	    {
            _authentificationService = authentificationService;
	    }

	    public async Task<bool> StartCall (string message, string toPhoneNumber, string fromPhoneNumber = null,  string currentLocation = null)
        {
            var req = new PhoneCallRequest
            {
                ToPhoneNumber = toPhoneNumber,
                FromPhoneNumber = fromPhoneNumber,
                Message = message,
                CurrentLocation = currentLocation
            };

            var client = new RestClient(_authentificationService.BasicAuthToken);
		    var isSuccessful = await client.MakePhoneCall(req);
            return isSuccessful;
        }
	}
}
