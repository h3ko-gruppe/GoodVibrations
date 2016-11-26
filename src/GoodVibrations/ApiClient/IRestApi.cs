using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace GoodVibrations.ApiClient
{
    [Headers ("Accept: application/json")]
    public interface IRestApi
    {
        [Get ("/api/login/")]       
        Task<HttpResponseMessage> Login ([Header("Authorization")] string authorization);

        [Post ("/api/register/")]
        Task<HttpResponseMessage> CreateAccount ([Body]RegistrationRequest request);

        [Post ("/api/phonecall/")]
        Task<HttpResponseMessage> MakePhoneCall ([Header ("Authorization")]string authorization);
    }
}
