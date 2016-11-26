using System;
using GoodVibrations.Models.Base;
using SQLite.Net.Attributes;

namespace GoodVibrations.Core.Models
{
    [Table("UserUserModel")]
	public class UserModel : BaseModel
	{
		[Column("LoginName")]
		public string FirstName
		{
			get;
			set;
		}

		[Column("Passwort")]
		public string LastName
		{
			get;
			set;
		}
	}
}
