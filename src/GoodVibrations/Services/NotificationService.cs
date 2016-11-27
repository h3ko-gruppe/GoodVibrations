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

        public NotificationService (IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
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

                var message = $"Received sound '{notification.EventId}' on '{notification.Name}'.";
                await App.Current.MainPage.DisplayAlert("Soundnotification", message, "Ok");
            });


        }

    }
}
