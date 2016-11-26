using System;
using GoodVibrations.Models.Base;
using SQLite.Net.Attributes;

namespace GoodVibrations.Models
{
	[Table("PhoneCall")]
	public class PhoneCall : BaseModel
	{

		[Column("Name")]
		public string Name
		{
			get;
			set;
		}

		[Column("DestinationNumber")]
		public string DestinationNumber
		{
			get;
			set;
		}

		[Column("Icon")]
		public string Icon
		{
			get;
			set;
		}

		[Column("Text")]
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
