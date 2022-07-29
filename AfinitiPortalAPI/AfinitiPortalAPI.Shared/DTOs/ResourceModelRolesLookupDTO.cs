using System;
using System.Collections.Generic;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class ResourceModelRolesLookupDTO
    {
        public int Id { get; set; }
        public string RoleResourceModel { get; set; }
        public int RoleGroupId { get; set; }
        public string RoleGroupName { get; set; }
        public string PaycomSubDepartment { get; set; }
        public int? Status { get; set; }
        public int? IsDedicated { get; set; }
        public int? Shifts { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
    }


    public class ResourceModelRolesGroupDTO
    {
        public string RoleGroupName { get; set; }
        public int RoleGroupId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class SwitchProvidersDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SwitchPlatformsDTO
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
    }

    public class RmRegionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public int? SubDepartmentId { get; set; }
        public string SubDepartment { get; set; }
        public int? TeamId { get; set; }
        public string Team { get; set; }
        public bool IsRegion { get; set; }
        public string RegionalManager { get; set; }
        public bool IsImmutable { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class RmProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
    public class RmRevenueBracketDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Maximum { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class RmAccountRegionalMappingDTO
    {
        public int Id { get; set; }
        public int? RegionId { get; set; }
        public int AccountId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
    public class RmEmployeeRegionalMappingDTO
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string EmployeeEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
    public class RmRoleDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int DepartmentId { get; set; }
        public int? SubdepartmentId { get; set; }
        public int? TeamId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int ShiftCount { get; set; }
    }

    public class RmRoleDTOTrimmed
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class IGenericLookup
    {
        public int value { get; set; }
        public string label { get; set; }
        public bool checkStatus { get; set; }
    }
    public class IGenericLookupWithParent : IGenericLookup
    {
        public int? parent { get; set; }
    }
    public class RmEmployeeRoleMappingFullDTO
    {
        public string Name { get; set; }
        public string EmployeeEmail { get; set; }
        public string TeamName { get; set; }
        public string PositionTitle { get; set; }
        public List<IGenericLookup> Roles { get; set; }
    }
    public class RmEmployeeRoleMappingDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int RoleId { get; set; }
        public string EmployeeEmail { get; set; }
        public bool IsActive { get; set; }
    }

    public class RmDepartmentDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Name { get; set; }
        public string PaycomDepartmentName { get; set; }
        public string PaycomDepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
    public class RmSubdepartmentDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string PaycomSubdepartmentName { get; set; }
        public string PaycomSubdepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
    public class RmTeamDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Name { get; set; }
        public int SubdepartmentId { get; set; }
        public bool IsActive { get; set; }

    }
}

