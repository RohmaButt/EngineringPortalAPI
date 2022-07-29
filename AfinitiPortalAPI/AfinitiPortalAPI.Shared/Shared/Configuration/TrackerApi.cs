using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Shared.Configuration
{
    public class TrackerApi
    {
        public string BaseApiUrl { get; set; }

        public int BatchPostingLimit { get; set; } = 1000;

        public int BatchPeriodInSeconds { get; set; } = 30;

        public string Schema { get; set; }

        public string Table { get; set; }
    }
}
