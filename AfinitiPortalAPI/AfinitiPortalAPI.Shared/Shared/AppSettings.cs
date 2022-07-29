using AfinitiPortalAPI.Shared.Shared.Configuration.Caching;

namespace AfinitiPortalAPI.Shared.Shared.Configuration
{
    public class AppSettings
    {
        public string CSTS_API_URL { get; set; }

        public string DBConnectionString { get; set; }

        public string[] Portal_Client_URL { get; set; }

        public Cache Cache { get; set; }

        public TrackerApi TrackerApi { get; set; }
    }
}
