using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Extensions
{
    public static class DecimalExtensions
    {
        public static bool IsWholeNumber(this decimal input)
        {
            return input % 1 == 0;
        }
    }
}
