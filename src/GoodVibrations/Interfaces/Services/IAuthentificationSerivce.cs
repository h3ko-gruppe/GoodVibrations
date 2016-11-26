using System;
using System.Threading.Tasks;
using GoodVibrations.Core.Models;

namespace GoodVibrations.Interfaces.Services
{
	public interface IAuthentificationSerivce
	{
		Task <bool> Login (string username, string password);
        Task<bool> Register (string username, string password);
   }
}