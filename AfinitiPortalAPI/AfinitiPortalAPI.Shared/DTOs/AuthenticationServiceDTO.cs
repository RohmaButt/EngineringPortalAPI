using System;
using System.Collections.Generic;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class RootLoginRequestModel
    {
        public LoginRequestModel model { get; set; }
    }
    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public string CrowdSSOToken { get; set; }
        public string ControllerPath { get; set; }
    }
    public class RootObject
    {
        public Response Response { get; set; }
    }
    public class Response
    {
        public string CrowdSSOToken { get; set; }
        public string Email { get; set; }
        public int AuthenticationCode { get; set; }
        public short ApprovalStatus { get; set; }
        public string UserName { get; set; }
        public string UserKey { get; set; }
        public string RemoteKey { get; set; }
        public string JSessionID { get; set; }
        public string AuthenticationMetaData { get; set; }
    }

    public class Root
    {
        public ResponseObj Response { get; set; }
    }
    public class ResponseObj
    {
        public string UserName { get; set; }
        public List<MenuPermission> Permissions { get; set; }
        public Guid RoleKey { get; set; }
        public Guid UserKey { get; set; }
        public string RoleName { get; set; }
        public int ApprovalStatus { get; set; }
        public Guid UserRemoteKey { get; set; }
        public bool LoggedInFromNewIP { get; set; }
        public CrowdUserObj CrowdObj { get; set; }
        public string Email { get; set; }
        public string OutlookGenieAttrMapping { get; set; }
        public bool IsAdmin { get; set; }
        public UserObj userObj { get; set; }
        public bool CanImpersonate { get; set; }
        public string ImpersonatedCrowdToken { get; set; }
    }
    public class UserObj
    {
        public string DisplayName { get; set; }
        public string AvatarURL { get; set; }
    }
    public class CrowdUserObj
    {
        public string CrowdSSOToken { get; set; }
        public string Email { get; set; }
        public int AuthenticationCode { get; set; }
        public short ApprovalStatus { get; set; }
        public string UserName { get; set; }
        public string UserKey { get; set; }
        public string RemoteKey { get; set; }
        public string JSessionID { get; set; }
        public string AuthenticationMetaData { get; set; }
    }
}
