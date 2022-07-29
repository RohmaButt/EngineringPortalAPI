using AfinitiPortalAPI.Shared.Authorization;
using AfinitiPortalAPI.Shared.DTOs;
using System;
using System.Collections.Generic;

namespace AfinitiPortalAPI.Shared
{
    public class CrowdResult
    {
        public string Email { get; set; }
        public string CrowdSSOToken { get; set; }
        public string UserName { get; set; }
        public bool IsValid { get; set; }//AuthenticationCode
        public string ResponseMessage { get; set; }//AuthenticationMetaData
        public string JSessionID { get; set; }

    }
    public class AuthResult
    {
        public string JSessionId { get; set; }
        public string CrowdToken { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public PaycomUserObjInfo PaycomUserObjInfo { get; set; }
        public List<PermissionModel> UserProfileData { get; set; }
    }

    public class PaycomUserObjInfo
    {
        public string Employee_Code { get; set; }
        public string Work_Email { get; set; }
        public string Position { get; set; }
        public string Position_Code { get; set; }
        public string Department { get; set; }
        public string Role_Name { get; set; }
        public string Level { get; set; }
        public string People_Manager { get; set; }
        public string TwoLevelBelowCeoCode { get; set; }
        public string WorkStatus { get; set; }
        public string ChangeStatus { get; set; }
        public string FirstSupervisorCode { get; set; }
        public string FirstSupervisor { get; set; }
        public string FirstSupervisorWorkStatus { get; set; }
        public string FirstSupervisorDepartment { get; set; }
        public string FirstSupervisorWorkEmail { get; set; }
    }
}
