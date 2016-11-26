using System;
using GoodVibrations.Models.Base;
using ReactiveUI.Fody.Helpers;
using SQLite.Net.Attributes;

namespace GoodVibrations.Models
{
	[Table("Notification")]
	public class Notification : BaseModel
	{
		[Column("Name")]
        [Reactive]
        public string Name
		{
			get;
			set;
		}

        [Column("QrCode")]
        [Reactive]
        public string QrCode
        {
            get;
            set;
        }

        [Column ("EventId")]
        [Reactive]
        public string EventId {
            get;
            set;
        }

		[Column("Sound")]
        [Reactive]
		public string SoundFilePath
		{
			get;
			set;
		}

		[Column("Active")]
        [Reactive]
		public bool Active
		{
			get;
			set;
		}

		[Column("NotificationIcon")]
        [Reactive]
		public string NotificationIcon
		{
			get;
			set;
		}
	}
}
