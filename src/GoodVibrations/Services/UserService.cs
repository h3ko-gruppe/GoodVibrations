using System;
using System.Threading.Tasks;
using GoodVibrations.Core.Models;
using GoodVibrations.Interfaces.Services;

namespace GoodVibrations.Services
{
	public class UserService : IUserService
	{
		public UserService()
		{
		}

		public Task Login(UserModel user)
		{
			throw new NotImplementedException();
		}
	}
}
