using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs.KPIDashboard
{
    public class GetComboDataForOkrResponse
    {
        public List<GetComboDataForOkrResponsePortfolioResponseItem> PortfolioFilterData { get; set; } = new List<GetComboDataForOkrResponsePortfolioResponseItem>();

        public List<GetComboDataForOkrResponseServiceGroupResponseItem> ServiceGroupFilterData { get; set; } = new List<GetComboDataForOkrResponseServiceGroupResponseItem>();

        public List<GetComboDataForOkrResponseServiceResponseItem> ServiceFilterData { get; set; } = new List<GetComboDataForOkrResponseServiceResponseItem>();
    }

    public class GetComboDataForOkrResponsePortfolioResponseItem
    {
        public string Portfolio { get; set; }
    }

    public class GetComboDataForOkrResponseServiceGroupResponseItem
    {
        public string Portfolio { get; set; }

        public string ServiceGroup { get; set; }
    }

    public class GetComboDataForOkrResponseServiceResponseItem
    {
        public string Portfolio { get; set; }

        public string ServiceGroup { get; set; }

        public string Service { get; set; }
    }
}
