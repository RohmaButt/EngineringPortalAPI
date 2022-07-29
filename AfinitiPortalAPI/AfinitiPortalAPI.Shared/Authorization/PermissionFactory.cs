using AfinitiPortalAPI.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Authorization
{
    public class PermissionFactory
    {
#warning There should be a user permission table instead of below static values normally. It's a temporary solution for quick win. In near future, We will be moving authorization to Portal's own DB context and creating required tables user authorization. akay 12-01-21

        public bool CanImpersonate(string email)
        {
            return new List<string>() {
                "akay.eskicirak@afiniti.com",
                "firat.ada@afiniti.com",
                "rohma.butt@afiniti.com",
                "alper.mestci@afiniti.com",
                "cevat.erkibas@afiniti.com",
                "bugra.erdogan@afiniti.com",
                "beste.oztamur@afiniti.com",
                "ezgi.uguzalp@afiniti.com",
                "efsun.bal@afiniti.com",
                "burak.gezer@afiniti.com",
                "fayaz.alam@afiniti.com",
                "baris.ozcifci@afiniti.com",
                "ebru.toros@afiniti.com",
                "beril.bosnalier@afiniti.com"
            }.Where(x => string.Equals(x, email, StringComparison.OrdinalIgnoreCase)).Count() > 0;
        }

        public bool IsTimeKeepingAdmin(string email)
        {
            return new List<string>() {
                "colin.miles@afiniti.com",
                "alper.mestci@afiniti.com",
                //"tayyaba.rizwan@afiniti.com",
            }.Where(x => string.Equals(x, email, StringComparison.OrdinalIgnoreCase)).Count() > 0;
        }

        public bool IsMemberOfBusinessIntelligenceTeam(string email)
        {
            return new List<string>()
            {
                "akay.eskicirak@afiniti.com",
                "firat.ada@afiniti.com",
                "rohma.butt@afiniti.com",
                "alper.mestci@afiniti.com",
                "cevat.erkibas@afiniti.com",
                "ezgi.uguzalp@afiniti.com",
                "bugra.erdogan@afiniti.com",
                "tayyaba.rizwan@afiniti.com"
            }.Where(x => string.Equals(x, email, StringComparison.OrdinalIgnoreCase)).Count() > 0;
        }

        public NavigationMenu NavigationMenuFactory { get; set; } = new NavigationMenu();

#warning We really need to move below menu data to DB asap. For now, we will be managing it from ONLY this class. akay 25-03-22
        public class NavigationMenu
        {
            public List<MenuPermissionModel> FullyGrantedProfile
            {
                get
                {
                    return new List<MenuPermissionModel>()
                    {
                        HOME,
                        MY_CAREER,
                        CAREER_FRAMEWORK,
                        ALL_ROLES,
                        WHAT_IS_IMPACT,
                        CORE_COMPETENCIES,
                        MY_TEAM,
                        ORG_CHART,

                        MY_GEEK_TOOLS,
                        API_PORTAL,
                        RELATIONSHIPS,
                        SERVICE_CATALOGUE,
                        ENTERPRISE_ARCHITECTURE_BOARD,
                        MY_RESOURCES,
                        AGILE_AT_AFINITI,
                        SIZING,
                        ROLES_IN_ACTION,
                        PRODUCT_OWNER,
                        HEAD_OF_ENGINEERING,
                        SCRUM_MASTER,
                        SERVICE_GROUP_LEAD,
                        SERVICE_LEAD,
                        DEVELOPER,
                        SDET,
                        QA,
                        ENTERPRISE_ARCHITECT,
                        DISCOURSE,
                        LEARNING,
                        ENGINEERING_PRINCIPLES,

                        SID_RESOURCE_PLANNING,
                        SID_REGION_MAPPING,
                        SID_REGIONS_FOR_ACCOUNT,
                        SID_REGIONS_FOR_EMPLOYEE,
                        SID_ROLES_FOR_EMPLOYEE,
                        WORKFLOW_FOR_APPROVAL,
                        RESOURCE_MODEL_ROLES,
                        EMPLOYEE_SWITCH_KNOWLEDGE,

                        GSD_RESOURCE_MODEL_DEFINITIONS,
                        GSD_RESOURCE_MODEL_ROLES,
                        GSD_RESOURCE_MODEL_REVENUE_BRACKETS,
                        GSD_RESOURCE_MODEL_PRODUCTS,

                        TIMEKEEPING_IN_PERCENT,
                        TIMEKEEPING_IN_PERCENT_TIMESHEET,
                        TIMEKEEPING_IN_PERCENT_PERIOD_MANAGEMENT,
                        TIMEKEEPING_IN_PERCENT_ADMINISTRATION
                    };
                }
            }

            /// <summary>
            /// Default Permission set with minimum number of menus.
            /// </summary>
            public List<MenuPermissionModel> Profile_1
            {
                get
                {
                    return new List<MenuPermissionModel>()
                    {
                        HOME,
                        MY_CAREER,
                        CAREER_FRAMEWORK,
                        ALL_ROLES,
                        WHAT_IS_IMPACT,
                        CORE_COMPETENCIES,
                        MY_TEAM,
                        ORG_CHART
                    };
                }
            }

            public List<MenuPermissionModel> Profile_2
            {
                get
                {
                    return new List<MenuPermissionModel>()
                    {
                        HOME,
                        MY_CAREER,
                        CAREER_FRAMEWORK,
                        ALL_ROLES,
                        WHAT_IS_IMPACT,
                        CORE_COMPETENCIES,
                        MY_TEAM,
                        ORG_CHART,
                        MY_GEEK_TOOLS,
                        API_PORTAL,
                        RELATIONSHIPS,
                        SERVICE_CATALOGUE,
                        ENTERPRISE_ARCHITECTURE_BOARD,
                        MY_RESOURCES,
                        AGILE_AT_AFINITI,
                        SIZING,
                        ROLES_IN_ACTION,
                        PRODUCT_OWNER,
                        HEAD_OF_ENGINEERING,
                        SCRUM_MASTER,
                        SERVICE_GROUP_LEAD,
                        SERVICE_LEAD,
                        DEVELOPER,
                        SDET,
                        QA,
                        ENTERPRISE_ARCHITECT,
                        DISCOURSE,
                        LEARNING,
                        ENGINEERING_PRINCIPLES
                    };
                }
            }

           /// <summary>
           /// Profile 1 + TimeKeeping In Percent Public Menus
           /// </summary>
            public List<MenuPermissionModel> Profile_3
            {
                get
                {
                    return new List<MenuPermissionModel>()
                    {
                        HOME,
                        MY_CAREER,
                        CAREER_FRAMEWORK,
                        ALL_ROLES,
                        WHAT_IS_IMPACT,
                        CORE_COMPETENCIES,
                        MY_TEAM,
                        ORG_CHART,
                        TIMEKEEPING_IN_PERCENT,
                        TIMEKEEPING_IN_PERCENT_TIMESHEET
                    };
                }
            }

            public MenuPermissionModel HOME = new MenuPermissionModel()
            {
                Title = "Home",
                PermissionKey = 1,
                ParentKey = null,
                LinkTo = "/kpi",
                HasBullet = false,
                Icon = "/media/icons/duotune/art/art002.svg",
                FontIcon = "bi-app-indicator",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 1,
                IsActive = true
            };

            public MenuPermissionModel MY_CAREER = new MenuPermissionModel()
            {
                Title = "My Career",
                PermissionKey = 2,
                ParentKey = null,
                LinkTo = "/my-career",
                HasBullet = false,
                Icon = "/media/menuIcons/briefcase-solid.svg",
                FontIcon = "bi-app-indicator",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 2,
                IsActive = true
            };

            public MenuPermissionModel CAREER_FRAMEWORK = new MenuPermissionModel()
            {
                Title = "Career Framework",
                PermissionKey = 3,
                ParentKey = 2,
                LinkTo = "/my-career/career-framework",
                HasBullet = false,
                Icon = "/media/menuIcons/flag-solid.svg",
                FontIcon = "bi-app-indicator",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 3,
                IsActive = true
            };

            public MenuPermissionModel ALL_ROLES = new MenuPermissionModel()
            {
                Title = "All Roles",
                PermissionKey = 4,
                ParentKey = 2,
                LinkTo = "/my-career/all-roles-engineering",
                HasBullet = false,
                Icon = "/media/menuIcons/id-card-alt-solid.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 4,
                IsActive = true
            };

            public MenuPermissionModel WHAT_IS_IMPACT = new MenuPermissionModel()
            {
                Title = "What is Impact?",
                PermissionKey = 5,
                ParentKey = 2,
                LinkTo = "/my-career/what-is-impact",
                HasBullet = false,
                Icon = "/media/icons/duotune/art/art002.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 5,
                IsActive = true
            };

            public MenuPermissionModel CORE_COMPETENCIES = new MenuPermissionModel()
            {
                Title = "Core Competencies",
                PermissionKey = 6,
                ParentKey = 2,
                LinkTo = "/my-career/core-competencies",
                HasBullet = false,
                Icon = "/media/menuIcons/abs026.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 6,
                IsActive = false
            };

            public MenuPermissionModel MY_TEAM = new MenuPermissionModel()
            {
                Title = "My Team",
                PermissionKey = 7,
                ParentKey = null,
                LinkTo = "/my-team",
                HasBullet = false,
                Icon = "/media/menuIcons/users-solid.svg",
                FontIcon = "bi-person",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 7,
                IsActive = true
            };

            public MenuPermissionModel ORG_CHART = new MenuPermissionModel()
            {
                Title = "Org Chart",
                PermissionKey = 8,
                ParentKey = 7,
                LinkTo = "/my-team/orgchart",
                HasBullet = false,
                Icon = "/media/menuIcons/org.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 8,
                IsActive = true
            };

            public MenuPermissionModel MY_GEEK_TOOLS = new MenuPermissionModel()
            {
                Title = "My Geek Toolbox",
                PermissionKey = 9,
                ParentKey = null,
                LinkTo = "/my-toolbox",
                HasBullet = false,
                Icon = "/media/menuIcons/tools-solid.svg",
                FontIcon = "bi-sticky",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 9,
                IsActive = true
            };

            public MenuPermissionModel API_PORTAL = new MenuPermissionModel()
            {
                Title = "API Portal",
                PermissionKey = 10,
                ParentKey = 9,
                LinkTo = "/my-toolbox/api-portal",
                HasBullet = false,
                Icon = "/media/icons/duotune/coding/cod001.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 10,
                IsActive = true
            };

            public MenuPermissionModel RELATIONSHIPS = new MenuPermissionModel()
            {
                Title = "Relationships",
                PermissionKey = 11,
                ParentKey = 9,
                LinkTo = "/my-toolbox/relationship",
                HasBullet = false,
                Icon = "/media/menuIcons/org.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 11,
                IsActive = true
            };

            public MenuPermissionModel SERVICE_CATALOGUE = new MenuPermissionModel()
            {
                Title = "Service Catalog",
                PermissionKey = 12,
                ParentKey = 9,
                LinkTo = "/my-toolbox/service-catalogue",
                HasBullet = false,
                Icon = "/media/menuIcons/battle-net-brands.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 12,
                IsActive = true
            };

            public MenuPermissionModel ENTERPRISE_ARCHITECTURE_BOARD = new MenuPermissionModel()
            {
                Title = "Enterprise Architecture Board",
                PermissionKey = 13,
                ParentKey = 9,
                LinkTo = "https://connect.afiniti.com/display/EG/1.++Enterprise+Architecture+Board",
                HasBullet = false,
                Icon = "/media/menuIcons/link-arrow.svg",
                FontIcon = "",
                IsNewTab = true,
                IsLandingPage = false,
                SortOrder = 13,
                IsActive = true
            };

            public MenuPermissionModel MY_RESOURCES = new MenuPermissionModel()
            {
                Title = "My Resources",
                PermissionKey = 14,
                ParentKey = null,
                LinkTo = "/my-resources",
                HasBullet = false,
                Icon = "/media/menuIcons/building-regular.svg",
                FontIcon = "bi-layers",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 14,
                IsActive = true
            };

            public MenuPermissionModel AGILE_AT_AFINITI = new MenuPermissionModel()
            {
                Title = "Agile @Afiniti",
                PermissionKey = 15,
                ParentKey = 14,
                LinkTo = "/my-resources/agile-afiniti",
                HasBullet = false,
                Icon = "/media/menuIcons/arr056.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 15,
                IsActive = true
            };

            public MenuPermissionModel SIZING = new MenuPermissionModel()
            {
                Title = "Sizing",
                PermissionKey = 16,
                ParentKey = 14,
                LinkTo = "/my-resources/sizing",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 16,
                IsActive = true
            };

            public MenuPermissionModel ROLES_IN_ACTION = new MenuPermissionModel()
            {
                Title = "Roles in Action",
                PermissionKey = 17,
                ParentKey = 14,
                LinkTo = "/my-resources",
                HasBullet = false,
                Icon = "/media/icons/duotune/general/gen040.svg",
                FontIcon = "bi-sticky",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 17,
                IsActive = true
            };

            public MenuPermissionModel PRODUCT_OWNER = new MenuPermissionModel()
            {
                Title = "Product Owner(PO)",
                PermissionKey = 18,
                ParentKey = 17,
                LinkTo = "/my-resources/product-owner",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 18,
                IsActive = true
            };

            public MenuPermissionModel HEAD_OF_ENGINEERING = new MenuPermissionModel()
            {
                Title = "Head Of Engineering",
                PermissionKey = 19,
                ParentKey = 17,
                LinkTo = "/my-resources/head-engineering",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 19,
                IsActive = true
            };

            public MenuPermissionModel SCRUM_MASTER = new MenuPermissionModel()
            {
                Title = "Scrum Master",
                PermissionKey = 20,
                ParentKey = 17,
                LinkTo = "/my-resources/scrum-master",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 20,
                IsActive = true
            };

            public MenuPermissionModel SERVICE_GROUP_LEAD = new MenuPermissionModel()
            {
                Title = "Service Group Lead",
                PermissionKey = 21,
                ParentKey = 17,
                LinkTo = "/my-resources/sg-lead",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 21,
                IsActive = true
            };

            public MenuPermissionModel SERVICE_LEAD = new MenuPermissionModel()
            {
                Title = "Service Lead",
                PermissionKey = 22,
                ParentKey = 17,
                LinkTo = "/my-resources/servicelead",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 22,
                IsActive = true
            };

            public MenuPermissionModel DEVELOPER = new MenuPermissionModel()
            {
                Title = "Developer",
                PermissionKey = 23,
                ParentKey = 17,
                LinkTo = "/my-resources/developer",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 23,
                IsActive = true
            };

            public MenuPermissionModel SDET = new MenuPermissionModel()
            {
                Title = "SDET",
                PermissionKey = 24,
                ParentKey = 17,
                LinkTo = "/my-resources/sdet",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 24,
                IsActive = true
            };

            public MenuPermissionModel QA = new MenuPermissionModel()
            {
                Title = "QA",
                PermissionKey = 25,
                ParentKey = 17,
                LinkTo = "/my-resources/qa",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 25,
                IsActive = true
            };

            public MenuPermissionModel ENTERPRISE_ARCHITECT = new MenuPermissionModel()
            {
                Title = "Enterprise Architect",
                PermissionKey = 26,
                ParentKey = 17,
                LinkTo = "/my-resources/e-architect",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 26,
                IsActive = true
            };

            public MenuPermissionModel DISCOURSE = new MenuPermissionModel()
            {
                Title = "Discourse",
                PermissionKey = 27,
                ParentKey = 14,
                LinkTo = "http://discuss.ai.afiniti.com",
                HasBullet = false,
                Icon = "/media/menuIcons/link-arrow.svg",
                FontIcon = "",
                IsNewTab = true,
                IsLandingPage = false,
                SortOrder = 27,
                IsActive = true
            };

            public MenuPermissionModel LEARNING = new MenuPermissionModel()
            {
                Title = "Learning",
                PermissionKey = 28,
                ParentKey = 14,
                LinkTo = "https://connect.afiniti.com/spaces/viewspace.action?key=ETND",
                HasBullet = false,
                Icon = "/media/menuIcons/link-arrow.svg",
                FontIcon = "",
                IsNewTab = true,
                IsLandingPage = false,
                SortOrder = 28,
                IsActive = true
            };

            public MenuPermissionModel ENGINEERING_PRINCIPLES = new MenuPermissionModel()
            {
                Title = "Engineering Principles",
                PermissionKey = 29,
                ParentKey = 14,
                LinkTo = "/my-resources/engineering-principles",
                HasBullet = false,
                Icon = "/media/menuIcons/briefcase-solid.svg",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 29,
                IsActive = true
            };

            public MenuPermissionModel SID_RESOURCE_PLANNING = new MenuPermissionModel()
            {
                Title = "SID Resource Planning",
                PermissionKey = 30,
                ParentKey = null,
                LinkTo = "/sid-resource-planning",
                HasBullet = false,
                Icon = "/media/menuIcons/users-solid.svg",
                FontIcon = "bi-person",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 30,
                IsActive = true
            };

            public MenuPermissionModel SID_REGION_MAPPING = new MenuPermissionModel()
            {
                Title = "SID - Region Setting",
                PermissionKey = 31,
                ParentKey = 30,
                LinkTo = "/sid-resource-planning/sid-region-mapping",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 31,
                IsActive = true
            };

            public MenuPermissionModel SID_REGIONS_FOR_ACCOUNT = new MenuPermissionModel()
            {
                Title = "SID - Accounts Regional Mapping",
                PermissionKey = 33,
                ParentKey = 30,
                LinkTo = "/sid-resource-planning/accounts-regional-mapping",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 32,
                IsActive = true
            };

            public MenuPermissionModel SID_REGIONS_FOR_EMPLOYEE = new MenuPermissionModel()
            {
                Title = "SID - Employees Regional Mapping",
                PermissionKey = 33,
                ParentKey = 30,
                LinkTo = "/sid-resource-planning/employees-regional-mapping",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 32,
                IsActive = true
            };

            public MenuPermissionModel SID_ROLES_FOR_EMPLOYEE = new MenuPermissionModel()
            {
                Title = "SID - Employees Role Mapping",
                PermissionKey = 33,
                ParentKey = 30,
                LinkTo = "/sid-resource-planning/employees-role-mapping",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 32,
                IsActive = true
            };

            public MenuPermissionModel WORKFLOW_FOR_APPROVAL = new MenuPermissionModel()
            {
                Title = "Workflow for Approval",
                PermissionKey = 32,
                ParentKey = 30,
                LinkTo = "/sid-resource-planning/workflow-for-approval",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 33,
                IsActive = true
            };

            public MenuPermissionModel RESOURCE_MODEL_ROLES = new MenuPermissionModel()
            {
                Title = "Resource Model Roles",
                PermissionKey = 34,
                ParentKey = 30,
                LinkTo = "/sid-resource-planning/resource-model-roles",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 34,
                IsActive = true
            };

            public MenuPermissionModel EMPLOYEE_SWITCH_KNOWLEDGE = new MenuPermissionModel()
            {
                Title = "Employee Switch Knowledge",
                PermissionKey = 35,
                ParentKey = 30,
                LinkTo = "/sid-resource-planning/employee-switch-knowledge",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 35,
                IsActive = true
            };

            /*GSD RESOURCE MODEL DEFINITIONS*/
            public MenuPermissionModel GSD_RESOURCE_MODEL_DEFINITIONS = new()
            {
                Title = "GSD Resource Model Definition",
                PermissionKey = 36,
                ParentKey = null,
                LinkTo = "/gsd-resource-model-definition",
                HasBullet = false,
                Icon = "/media/menuIcons/users-solid.svg",
                FontIcon = "bi-person",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 36,
                IsActive = true
            };

            public MenuPermissionModel GSD_RESOURCE_MODEL_ROLES = new()
            {
                Title = "Roles",
                PermissionKey = 37,
                ParentKey = 36,
                LinkTo = "/gsd-resource-model-definition/roles",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 37,
                IsActive = true
            };

            public MenuPermissionModel GSD_RESOURCE_MODEL_REVENUE_BRACKETS = new()
            {
                Title = "Revenue Brackets",
                PermissionKey = 41,
                ParentKey = 36,
                LinkTo = "/gsd-resource-model-definition/revenue-brackets",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 41,
                IsActive = true
            };

            public MenuPermissionModel GSD_RESOURCE_MODEL_PRODUCTS = new()
            {
                Title = "Products",
                PermissionKey = 42,
                ParentKey = 36,
                LinkTo = "/gsd-resource-model-definition/products",
                HasBullet = true,
                Icon = "",
                FontIcon = "",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 42,
                IsActive = true
            };

            /*France & Spain TimeKeeping Pages*/
            public MenuPermissionModel TIMEKEEPING_IN_PERCENT = new()
            {
                Title = "Afiniti Time Keeping",
                PermissionKey = 43,
                ParentKey = null,
                LinkTo = "/timekeeping-in-percent",
                HasBullet = false,
                Icon = "/media/menuIcons/users-solid.svg",
                FontIcon = "bi-person",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 43,
                IsActive = true
            };

            public MenuPermissionModel TIMEKEEPING_IN_PERCENT_TIMESHEET = new()
            {
                Title = "Timesheet",
                PermissionKey = 44,
                ParentKey = 43,
                LinkTo = "/timekeeping-in-percent/timesheet",
                HasBullet = true,
                Icon = "/media/menuIcons/users-solid.svg",
                FontIcon = "bi-person",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 44,
                IsActive = true
            };

            public MenuPermissionModel TIMEKEEPING_IN_PERCENT_PERIOD_MANAGEMENT = new()
            {
                Title = "Period Management",
                PermissionKey = 45,
                ParentKey = 43,
                LinkTo = "/timekeeping-in-percent/period-management",
                HasBullet = true,
                Icon = "/media/menuIcons/users-solid.svg",
                FontIcon = "bi-person",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 45,
                IsActive = true
            };

            public MenuPermissionModel TIMEKEEPING_IN_PERCENT_ADMINISTRATION = new()
            {
                Title = "Administration",
                PermissionKey = 46,
                ParentKey = 43,
                LinkTo = "/timekeeping-in-percent/administration",
                HasBullet = true,
                Icon = "/media/menuIcons/users-solid.svg",
                FontIcon = "bi-person",
                IsNewTab = false,
                IsLandingPage = false,
                SortOrder = 46,
                IsActive = true
            };
        }
    }
}
