using System;
using GoodVibrations.Models;

namespace GoodVibrations.EventArgs
{
    public class NotificationReceivedEventArgs
    {

        public NotificationReceivedEventArgs (string eventId, Notification notification)
        {
            EventId = eventId;
            Notification = notification;
        }

        public string EventId {
            get;
            private set;
        }

        public Notification Notification {
            get;
            private set;
        }

    }
}