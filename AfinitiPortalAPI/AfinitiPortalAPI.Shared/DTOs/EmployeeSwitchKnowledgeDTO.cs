using System;
using System.Collections.Generic;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class EmployeeSwitchKnowledgeDTO
    {
        public int Id { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeName { get; set; }
        public int? SwitchProviderId { get; set; }
        public string SwitchProviderName { get; set; }
        public int? SwitchPlatformId { get; set; }
        public int? SwitchPlatformProviderId { get; set; }
        public string SwitchPlatformName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsActive { get; set; }
    }


    public class EmployeeSwitchKnowledgeTrimmedDTO
    {
        public string EmployeeEmail { get; set; }
        public int? SwitchPlatformId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsActive { get; set; }
    }
    public class EmployeeSwitchKnowledgeGroupedDTO
    {
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public List<IGenericLookupWithParent> SwitchPlatforms { get; set; }
    }
    public class EmployeeSwitchKnowledgeHeaderDTO
    {
        public int SwitchProviderId { get; set; }
        public string SwitchProviderName { get; set; }
        public int SwitchPlatformId { get; set; }
        public int? SwitchPlatformProviderId { get; set; }
        public string SwitchPlatformName { get; set; }
    }
    public class EmployeeSwitchKnowledgeHeaderGroupedDTO
    {

        public int SwitchProviderId { get; set; }
        public string SwitchProviderName { get; set; }
        public List<IGenericLookup> SwitchPlatforms { get; set; }
    }
}
