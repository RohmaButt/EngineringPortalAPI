using System;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class TsPeriodDTO
    {
        public int Id { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public bool? LockStatus { get; set; }
        public DateTime InsertionDate { get; set; }
        public string ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class TsWorklogDTO
    {
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public string Week { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public float LoggedHoursPercent { get; set; }
        public string Description { get; set; }
        public sbyte? ApprovalStatus { get; set; }
        public string ApproverEmail { get; set; }
        public DateTime InsertionDate { get; set; }
        public string InsertionUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifierName { get; set; }
        public bool IsActive { get; set; }
    }
    public class TsWorklogTrimmedDTO
    {
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public string Week { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public float LoggedHoursPercent { get; set; }
        public string Description { get; set; }
    }
    public class TsWorklogAdminDTO
    {
        //public int Id { get; set; }
        public string EmployeeEmail { get; set; } //User
        public string CategoryName { get; set; }//Issue Category
        public string TypeName { get; set; }//Type
        public float Week1 { get; set; }
        public float Week2 { get; set; }
        public float Week3 { get; set; }
        public float Week4 { get; set; }
        public float Week5 { get; set; }
    }
    public class TsJiraIssueCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? HermesAccount { get; set; }
    }
    public class TsJiraIssueTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? IssueCategoryId { get; set; }
    }
    public class HermesOrganizationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
