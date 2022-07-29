using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Enums
{
    public enum PortalUserStatus
    {
        New = 0,
        Approved = 1,
        Rejected = 2,
        Pending = 3
    }

    public enum ApiResponseCode
    {
        Success = 0,
        NotFound = -1,
        AlreadyExists = -2,
        NotAuthorized = -3,

        Failure = -99
    }

    public enum KPIFixedColumn
    {
        Portfolio = 0,
        ServiceGroup = 1,
        Service = 2
    }

    public enum KPIDashboardMetricType
    {
        Quality,
        DeliverySpeed,
        ProductionQuality
    }

    public enum PermissionType
    {
        LandingPage = 1,
        NavigationMenu = 2,
        Component = 3
    }

    public enum KPIComboboxType
    {
        Portfolio,
        ServiceGroup,
        Service,
        KPIMainInterval
    }
}
