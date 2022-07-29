using AfinitiPortalAPI.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class GetChartDataRequestBase
    {
        public string Interval { get; set; }
    }

    public class GetChartDataRequest : GetChartDataRequestBase
    {
        public KPIFixedColumn FixedBy { get; set; }
    }

    public class GetBulkChartDataRequest : GetChartDataRequestBase
    {
        public KPIFixedColumn GraphsFixedBy { get; set; }

        public KPIFixedColumn TableFixedBy { get; set; }

        public GetChartDataRequest ToChartDataGraphRequest()
        {
            return new GetChartDataRequest()
            {
                FixedBy = this.GraphsFixedBy,
                Interval = this.Interval
            };
        }

        public GetChartDataRequest ToChartDatTableRequest()
        {
            return new GetChartDataRequest()
            {
                FixedBy = this.TableFixedBy,
                Interval = this.Interval
            };
        }
    }
}
