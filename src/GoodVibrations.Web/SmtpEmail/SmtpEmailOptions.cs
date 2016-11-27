using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodVibrations.Web.SmtpEmail
{
    public class SmtpEmailOptions
    {
        public string DefaultFrom { get; set; }
        public bool IsEnabled { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
