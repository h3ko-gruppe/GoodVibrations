using System.Threading.Tasks;

namespace GoodVibrations.Interfaces.Services
{
	public interface IAuthentificationSerivce
	{
		Task <bool> Login (string username, string password);
        Task<bool> CreateAccount (string username, string password);
        string BasicAuthToken { get; }
   }
}