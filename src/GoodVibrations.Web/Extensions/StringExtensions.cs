using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace GoodVibrations.Web.Extensions
{
    public static class StringExtensions
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static MailboxAddress[] ToMailBoxAddresses(this string val)
        {
            if (string.IsNullOrEmpty(val) || val.EndsWith(" "))
                return new MailboxAddress[0];
            else
            {
                var multipleEmailsInOneStringSeperators = new[] { ',', ';' };
                var result = val.Split(multipleEmailsInOneStringSeperators, StringSplitOptions.RemoveEmptyEntries).Select(x =>
                {
                    var emailSeperators = new[] { '<', '>', '(', ')', '[', ']', '{', '}' };
                    var mailAddressPart = x.Split(emailSeperators, StringSplitOptions.RemoveEmptyEntries);
                    return new MailboxAddress(mailAddressPart[0], mailAddressPart[1]);
                }).ToArray();
                return result;
            }
        }
        
    }
}
