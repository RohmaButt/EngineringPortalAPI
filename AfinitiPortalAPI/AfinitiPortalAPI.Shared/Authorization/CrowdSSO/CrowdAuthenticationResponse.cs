using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Authorization.CrowdSSO
{
    public class CrowdAuthenticationResponse
    {
        public AuthenticationResponseInnerObject Response { get; set; } = new AuthenticationResponseInnerObject();
    }
    
    public class AuthenticationResponseInnerObject
    {
        public  int ApprovalStatus { get; set; }

        public int AuthenticationCode { get; set; }

        public string AuthenticationMetaData { get; set; }

        public string CrowdSSOToken { get; set; }

        public string Email { get; set; }

        public string JSessionID { get; set; }

        public string RemoteKey { get; set; }

        public string UserKey { get; set; }

        public string UserName { get; set; }
    }
}
