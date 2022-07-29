using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared
{
    public static class Constants
    {
        public static class Api
        {
            public const string RequestDateFormat = "yyyyMMdd";

            public const string ResponseDateFormat = "MM/dd/yyyy";

            public const string CrowdTokenHeaderKey = "portal-token-key";

            public const string UserNameHeaderKey = "portal-user-name";
        }

        public static class Authorization
        {
            public const string LANDING_PAGE_COMPONENT_CODE = "LANDING_PAGE";

            public const string NAVIGATION_MENU_COMPONENT_CODE = "NAVIGATION_MENU";

            public const string COMPONENT_PREFIX = "C_";

            /// <summary>
            /// Components corresponds to reactJs components that contained by subModules and can vary per user at Portal UI.
            /// </summary>
            public static class Components
            {
                // KPI DASHBOARD
                public const string USER_CATALOGUE_INFO = COMPONENT_PREFIX + "USER_CAT_INFO";

                public const string KPI_MAIN_FILTER = COMPONENT_PREFIX + "KPI_UL_DDL";

                public const string KPI_GRAPHS = COMPONENT_PREFIX + "KPI_GRAHPS";

                public const string KPI_INTERVAL_FILTER = COMPONENT_PREFIX + "KPI_INT_DDL";

                public const string KPI_TABLE = COMPONENT_PREFIX + "KPI_TABLE";

                public const string OKR_TABLE = COMPONENT_PREFIX + "OKR_TABLE";

                public const string OKR_PORTFOLIO_FILTER = COMPONENT_PREFIX + "OKR_PFL_DDL";

                public const string OKR_SERVICE_GROUP_FILTER = COMPONENT_PREFIX + "OKR_SG_DDL";

                public const string OKR_INTERVAL_FILTER = COMPONENT_PREFIX + "OKR_INT_DDL";

                // UPPER BAR
                public const string PORTAL_IMPERSONATION_COMPONENT = COMPONENT_PREFIX + "EP_IMP_COMP";
            }

            /// <summary>
            /// Path for every menu item for Portal UI.
            /// </summary>
            public static class LandingPages
            {
                public const string KPI = "/kpi";

                public const string MyCareer = "/my-career/career-framework";
            }
        }

        public static class Paycom
        {
            public static class DepartmentCodes
            {
                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: Department
                /// Value:  GSD
                /// </summary>
                public const string GSD = "1T106000";
            }

            public static class SubdepartmentCodes
            {
                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: [Sub Department]
                /// Value:  AIDI
                /// </summary>
                public const string AIDI = "2T106005";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: [Sub Department]
                /// Value:  Engineering
                /// </summary>
                public const string Engineering = "2T106280";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: [Sub Department]
                /// Value:  Engineering Operations
                /// </summary>
                public const string EngineeringOperations = "2T106600";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: [Sub Department]
                /// Value:  SID
                /// </summary>
                public const string SID = "2T106400";
            }

            public static class RoleNames
            {
                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: Role_Name
                /// Value:  Head of Engineering
                /// </summary>
                public const string HeadOfEngineering = "Head of Engineering";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: Role_Name
                /// Value:  Service Group Lead
                /// </summary>
                public const string ServiceGroupLead = "Service Group Lead";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: Role_Name
                /// Value:  Service Group Architect
                /// </summary>
                public const string ServiceGroupArchitect = "Service Group Architect";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: Role_Name
                /// Value:  Service Group Manager
                /// </summary>
                public const string ServiceGroupManager = "Service Group Manager";
            }

            public static class Levels
            {
                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: LEVEL
                /// Value:  N
                /// </summary>
                public const string N = "N";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: LEVEL
                /// Value:  N-1
                /// </summary>
                public const string N_1 = "N-1";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: LEVEL
                /// Value:  N-2
                /// </summary>
                public const string N_2 = "N-2";
            }

            public static class Countries
            {
                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: COUNTRY
                /// Value:  France
                /// </summary>
                public const string France = "France";

                /// <summary>
                /// Table:  dbo.PAYCOM_EMPLOYEE_FULL
                /// Column: COUNTRY
                /// Value:  Spain
                /// </summary>
                public const string Spain = "Spain";
            }
        }

        public static class DB
        {
            public static class dbo
            {
                public const string PORTAL_OKR_BACKEND_OKR_LEVEL_INHERITED = "INHERITED OKR";

                public const string PORTAL_OKR_BACKEND_OKR_LEVEL_ONLY_PORTFOLIO = "Only Portfolio Level OKR";

                public const string PORTAL_OKR_BACKEND_OKR_LEVEL_ONLY_SERVICEGROUP = "Only Service Group Level OKR";
            }
        }
    }
}
