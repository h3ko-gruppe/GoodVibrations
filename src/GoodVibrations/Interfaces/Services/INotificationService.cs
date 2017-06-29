using System;
using System.Threading.Tasks;
using GoodVibrations.EventArgs;

namespace GoodVibrations.Interfaces.Services
{
	public interface INotificationService
	{
		event EventHandler<NotificationReceivedEventArgs> NotificationReceived;
	    Task ConnectToSignalRHub();
        Task ConnectToMsBand ();

	}
}