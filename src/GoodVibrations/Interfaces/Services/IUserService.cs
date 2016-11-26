using System;
using System.Threading.Tasks;
using GoodVibrations.Core.Models;

namespace GoodVibrations.Interfaces.Services
{
	public interface IUserService
	{
		Task Login(UserModel user);
	}
}
