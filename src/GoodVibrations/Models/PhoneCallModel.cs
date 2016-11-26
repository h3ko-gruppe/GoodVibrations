using System;
using SQLite.Net.Attributes;

namespace GoodVibrations.Models
{
	[Table("PhoneCallModel")]
	public class PhoneCallModel
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
