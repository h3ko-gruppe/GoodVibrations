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
            var hubConnection = new HubConnection("https://goodvibrations-app.azurewebsites.net/");
            var stockTickerHubProxy = hubConnection.CreateHubProxy("NotifyHub");
            stockTickerHubProxy.On<string> ("Notify", eventId => OnNotificationReceived (eventId));
            await hubConnection.Start();
        }

	    public event EventHandler<NotificationRecievedEventArgs> NotificationReceived;

        private void OnNotificationReceived (string eventId)
        {
            Debug.WriteLine ($"SignalR notification recieved: {eventId}");
            if (NotificationReceived != null) {

                var notification = _persistenceService.Notification.LoadWhere(x => x.EventId == eventId).FirstOrDefault() ;
                var e = new NotificationRecievedEventArgs (eventId, notification);
                NotificationReceived (this, e);
            }
            Device.BeginInvokeOnMainThread (async() => {
                await App.Current.MainPage.DisplayAlert ("eventId", $"Notification recieved: {eventId}", "Ok");
            });


        }

    }
}
