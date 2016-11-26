using System;
using SQLite.Net.Attributes;

namespace GoodVibrations.Models
{
	[Table("SoundModel")]
	public class SoundModel
	{
		[Column("Description")]
		public string Description
		{
			get;
			set;
		}

		[Column("Sound")]
		public string Sound
		{
			get;
			set;
		}
	}
}
