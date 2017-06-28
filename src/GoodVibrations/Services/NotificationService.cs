using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using GoodVibrations.EventArgs;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;
using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms;
using GoodVibrations.Consts;

namespace GoodVibrations.Services
{
	public class NotificationService : INotificationService
    {
        private readonly IPersistenceService _persistenceService;
        private readonly IMicrosoftBandService _microsoftBandService;
       
		public NotificationService (IPersistenceService persistenceService, IMicrosoftBandService microsoftBandService) 
		{
            _persistenceService = persistenceService;
			_microsoftBandService = microsoftBandService;
		}    

        public async Task ConnectToSignalRHub()
	    {
            var hubConnection = new HubConnection(Constants.RestApi.HostUrl);
            var stockTickerHubProxy = hubConnection.CreateHubProxy("NotifyHub");
            stockTickerHubProxy.On<string> ("Notify", eventId => OnNotificationReceived (eventId));
            await hubConnection.Start();
        }

        public async Task ConnectToMsBand ()
        {
            await _microsoftBandService.ConnectAndReadData();
        }
	    public event EventHandler<NotificationRecievedEventArgs> NotificationReceived;

        private void OnNotificationReceived (string eventId)
        {
            Debug.WriteLine ($"SignalR notification recieved: {eventId}");
            Device.BeginInvokeOnMainThread(async () =>
            {
                var notification = _persistenceService.Notification.LoadWhere(x => x.EventId == eventId).FirstOrDefault() ;

                if (notification?.Active != true)
                    return;

                if (NotificationReceived != null)
                {
                    var e = new NotificationRecievedEventArgs(eventId, notification);
                    NotificationReceived(this, e);
                }

                await _microsoftBandService.NotifyIfConnected (eventId, notification.Name);

                var message = $"Received sound '{notification.EventId}' on '{notification.Name}'.";
                await App.Current.MainPage.DisplayAlert("Soundnotification", message, "Ok");
            });


        }

    }
}
