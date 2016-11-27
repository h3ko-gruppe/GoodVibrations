using System;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.EventArgs;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Models;

namespace GoodVibrations.Services
{
	public class NotificationService : INotificationService
    {
        private readonly IPersistenceService _persistenceService;

        public NotificationService (IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
        }

        public event EventHandler<NotificationRecievedEventArgs> NotificationReceived;

        public async void OnNotificationReceived (string eventId)
        {
            if (NotificationReceived != null) {
                
                var notification = _persistenceService.Notification.LoadWhere(x => x.EventId == eventId).FirstOrDefault() ;
                var e = new NotificationRecievedEventArgs (eventId, notification);
                NotificationReceived (this, e);
                await App.Current.MainPage.DisplayAlert ("eventId", $"Notification recieved: {eventId}", "Ok");
            }
        }

    }
}
