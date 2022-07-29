using System.Collections.Generic;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class PaycomEmployeeFull_DTOFlat
    {
        public string nodeId { get; set; }//EmployeeCode///////////////id
        public string WorkEmail { get; set; }//
        public string name { get; set; }//employeeName
        public string positionName { get; set; }//PositionTitle
        public string WorkStatus { get; set; }
        public string FirstSupervisorWorkEmail { get; set; }
        public string FirstSupervisorWorkStatus { get; set; }
        public string parentNodeId { get; set; }//FirstSupervisorCode/////parentId
        //public string Gender { get; set; }
        //public string Url { get; set; }
        //public string Country { get; set; }
        //public string Location { get; set; }
        public string Department { get; set; }
        public string SubDepartment { get; set; }
        public string Team { get; set; }
        public string UserName { get; set; }
        //public string Role { get; set; }
        //public string Portfolio { get; set; }
        //public string ServiceGroup { get; set; }
        //public string Service { get; set; }
        public string LegalEntity { get; set; }
        public int? TotalHeadcount { get; set; }
        public int? DirectReports { get; set; }
        public string Level { get; set; }
        public bool SideChartFlag { get; set; }

    }

    public class PaycomEmployeeFull_DTO
    {
        public string EmployeeCode { get; set; }
        public string WorkEmail { get; set; }
        //public string FullName { get; set; }
        public string EmployeeName { get; set; }
        //public string Nickname { get; set; }
        //public string Firstname { get; set; }
        //public string Middlename { get; set; }
        //public string Lastname { get; set; }
        //public string WorkStatus { get; set; }
        //public DateTime? HireDate { get; set; }
        //public DateTime? TerminationDate { get; set; }
        //public DateTime? FirstHireDate { get; set; }
        //public string TerminationMonth { get; set; }
        //public DateTime? RehireDate { get; set; }
        //public string HireMonth { get; set; }
        //public string Department { get; set; }
        //public string SubDepartment { get; set; }
        //public string Team { get; set; }
        //public string Clocksequencenumber { get; set; }
        //public string EmploymentType { get; set; }
        public string PositionTitle { get; set; }
        //public string LocationRaw { get; set; }
        //public string Country { get; set; }
        //public int? CompanyLocationId { get; set; }
        //public string DolStatus { get; set; }
        //public string NineLevelBelowCeoCode { get; set; }
        //public string NineLevelBelowCeo { get; set; }
        //public string EigthLevelBelowCeoCode { get; set; }
        //public string EigthLevelBelowCeo { get; set; }
        //public string SevenLevelBelowCeoCode { get; set; }
        //public string SevenLevelBelowCeo { get; set; }
        //public string SixLevelBelowCeoCode { get; set; }
        //public string SixLevelBelowCeo { get; set; }
        //public string FiveLevelBelowCeoCode { get; set; }
        //public string FiveLevelBelowCeo { get; set; }
        //public string FourLevelBelowCeoCode { get; set; }
        //public string FourLevelBelowCeo { get; set; }
        //public string ThreeLevelBelowCeoCode { get; set; }
        //public string ThreeLevelBelowCeo { get; set; }
        //public string TwoLevelBelowCeoCode { get; set; }
        //public string TwoLevelBelowCeo { get; set; }
        //public string OneLevelBelowCeoCode { get; set; }
        //public string OneLevelBelowCeo { get; set; }
        //public string CeoCode { get; set; }
        //public string Ceo { get; set; }
        public string FirstSupervisorWorkEmail { get; set; }
        public string FirstSupervisorCode { get; set; }
        public string Gender { get; set; }
        public List<PaycomEmployeeFull_DTO> TeamOrgData { get; set; }
    }
    public class AfinitiUser
    {
        public string WorkEmail { get; set; }
        //removed extra properties to reduce load time
        //public string EmployeeName { get; set; }
        //public string NtLogin { get; set; }
        //public string WorkStatus { get; set; }

    }

    public class PaycomDepartments
    {
        public string SubDepartmentCode { get; set; }
        public string SubDepartmentDisplayName { get; set; }
    }
    public class EmployeeHeads_DTO
    {
        public string EmployeeCode { get; set; }
        public string WorkEmail { get; set; }
        public string NineLevelBelowCeoCode { get; set; }
        public string EigthLevelBelowCeoCode { get; set; }
        public string SevenLevelBelowCeoCode { get; set; }
        public string SixLevelBelowCeoCode { get; set; }
        public string FiveLevelBelowCeoCode { get; set; }
        public string FourLevelBelowCeoCode { get; set; }
        public string ThreeLevelBelowCeoCode { get; set; }
        public string TwoLevelBelowCeoCode { get; set; }
        public string OneLevelBelowCeoCode { get; set; }
        public string CeoCode { get; set; }
    }
    public class NewEmployeeSwitchKnowledgeDTO
    {
        public string WorkEmail { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string SubDepartment { get; set; }
        public string Team { get; set; }
        public string WorkStatus { get; set; }
        public string FirstSupervisorWorkEmail { get; set; }
        public string LegalCountry { get; set; }
        public string Country { get; set; }
    }

}
