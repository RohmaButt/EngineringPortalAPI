

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class OrgChartRequestObj
    {
        public string WorkEmail { get; set; }
        //public string TeamName { get; set; }
        public bool FetchTillLastEdge { get; set; } //True=all till last edge , False => only direct reportees 
        public string WorkStatus { get; set; }
        public int TreeStage { get; set; }
       public bool IsAdmin { get; set; }
    }
}
