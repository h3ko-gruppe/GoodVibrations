using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodVibrations.Web.BasicAuthentication
{
    public class BasicAuthenticationAttribute : BasicAuthenticationBaseAttribute
    {
        
        protected string Username { get; set; }
        protected string Password { get; set; }
         

        public BasicAuthenticationAttribute(string username, string password)
        {
            this.Username = username;
            this.Password = password; 
        }

        protected override bool Authorize(ActionExecutingContext actionExecutingContext, NetworkCredential credential)
        {
            return (string.Equals(credential.UserName, Username, StringComparison.OrdinalIgnoreCase) &&
                    credential.Password == Password);
        }
    }
}
