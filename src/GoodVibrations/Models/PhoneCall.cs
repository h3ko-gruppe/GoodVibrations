using System;
using GoodVibrations.Models.Base;
using ReactiveUI.Fody.Helpers;
using SQLite.Net.Attributes;

namespace GoodVibrations.Models
{
	[Table("PhoneCall")]
	public class PhoneCall : BaseModel
	{

		[Column("Name")]
        [Reactive]
		public string Name
		{
			get;
			set;
		}

		[Column("DestinationNumber")]
        [Reactive]
		public string DestinationNumber
		{
			get;
			set;
		}

		[Column("Icon")]
        [Reactive]
		public string Icon
		{
			get;
			set;
		}

		[Column("Text")]
        [Reactive]
		public string Text
		{
			get;
			set;
		}

		[Column("CurrentPosition")]
		public string CurrentPosition
		{
			get;
			set;
		}
	}
}
