using System;
using CoreLocation;
using System.Threading.Tasks;
using GoodVibrations.Interfaces;

namespace GoodVibrations.iOS.Utils
{
    public class LocationManager : ILocationManager
    {
        private readonly CLGeocoder _geoCoder;

        public LocationManager()
        {
            _geoCoder = new CLGeocoder();
        }

        public async Task<string> LoadAddress(double latitude, double longitude)
        {
            try
            {
                var address = await _geoCoder.ReverseGeocodeLocationAsync(new CLLocation(latitude, longitude));

                if (address != null && address.Length > 0)
                    return String.Format("{0}, {1} {2}", address[0].Name, address[0].PostalCode, address[0].AdministrativeArea);

				return "";
            }
            catch (Foundation.NSErrorException ex)
            {
				return "Cannot Locate";
            }
        }
    }
}