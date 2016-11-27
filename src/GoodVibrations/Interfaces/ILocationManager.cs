using System;

using System.Threading.Tasks;

namespace GoodVibrations.Interfaces
{
	public interface ILocationManager
	{
		Task<string> LoadAddress(double latitude, double longitude);
	}
}