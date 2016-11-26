using System;
using GoodVibrations.Core.Models;
using GoodVibrations.Models.Base;
using SQLite.Net.Attributes;

namespace GoodVibrations.Models
{
	[Table("Sound")]
	public class Sound : BaseModel
	{
		[Column("Description")]
		public string Description
		{
			get;
			set;
		}

		[Column("Sound")]
		public string SoundFilePath
		{
			get;
			set;
		}

		[Column("Active")]
		public string Active
		{
			get;
			set;
		}

		[Column("NotificationIcon")]
		public string NotificationIcon
		{
			get;
			set;
		}


		[Column("PersonWhoRings")]
		public User PersonWhoRings
		{
			get;
			set;
		}
	}
}
