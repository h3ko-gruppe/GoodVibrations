using System;
using GoodVibrations.Models.Base;
using SQLite.Net.Attributes;

namespace GoodVibrations.Core.Models
{
    [Table("User")]
	public class User : BaseModel
	{
		[Column("Name")]
		public string FirstName
		{
			get;
			set;
		}

		[Column("RingCount")]
		public int RingCount
		{
			get;
			set;
		}
	}
}
