using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace GoodVibrations.ApiClient
{
    public interface IRestApi
    {
        Task<HttpResponseMessage> Login ([Header("Authorization")] string authorization);

        Task<HttpResponseMessage> CreateAccount (string username, string password);

        Task<HttpResponseMessage> MakePhoneCall ([Header ("Authorization")]string authorization);
    }
}
