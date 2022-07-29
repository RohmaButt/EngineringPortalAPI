using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// For afiniti domain.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FromUserNameToEmail(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : $"{input}@afiniti.com";
        }
    }
}
