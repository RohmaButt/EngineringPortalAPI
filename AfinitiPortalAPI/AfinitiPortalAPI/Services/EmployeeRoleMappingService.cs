using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;
using AfinitiPortalAPI.Shared.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.services
{
    public interface IEmployeeRoleMappingService
    {
        Task<RmEmployeeRoleMappingDTO> AddOrUpdateEmployeeRoleMapping(RmEmployeeRoleMappingDTO mappingEntity);
        Task<dynamic> GetEmployeeRoleMapping();
    }
    public class EmployeeRoleMappingService : IEmployeeRoleMappingService
    {
        private readonly PortalDBContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeRoleMappingService(PortalDBContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<dynamic> GetEmployeeRoleMapping()
        {
            Log.Information("GetEmployeeRoleMapping Service:Started");
            //Using Query Syntax
            var leftOuterJoinData = await (
            from paycomEmployees in _dbContext.PaycomEmployeeFulls.AsNoTracking()
            join employeeRoleMapping in _dbContext.RmEmployeeRoleMappings.AsNoTracking() on paycomEmployees.WorkEmail equals employeeRoleMapping.EmployeeEmail into tempEmployeeRegionMapping
            from employeeRoleMapping in tempEmployeeRegionMapping.DefaultIfEmpty()
            join roles in _dbContext.RmRoles.AsNoTracking() on employeeRoleMapping.RoleId equals roles.Id into TempRoles
            from roles in TempRoles.DefaultIfEmpty()
            where paycomEmployees.ChangeStatus == "CURRENT STATUS" && paycomEmployees.WorkStatus == "WORKING" && paycomEmployees.SubDepartment == "SID"// && employeeRoleMapping.IsActive
            select new
            {
                Name = paycomEmployees.FullName,
                EmployeeEmail = paycomEmployees.WorkEmail,
                TeamName = paycomEmployees.Team,
                paycomEmployees.PositionTitle,
                RoleId = employeeRoleMapping.Id != null ? roles.Id : 99,
                RoleName = employeeRoleMapping.Id != null ? roles.Name : "No Role assigned",
                RoleStatus = employeeRoleMapping.Id != null ? employeeRoleMapping.IsActive : false
            }).ToListAsync().ConfigureAwait(false);

            var grouped = leftOuterJoinData
                .GroupBy(x => new { x.EmployeeEmail, x.TeamName, x.Name, x.PositionTitle })
                .Select(y => new RmEmployeeRoleMappingFullDTO()
                {
                    EmployeeEmail = y.Key.EmployeeEmail,
                    Name = y.Key.Name,
                    TeamName = y.Key.TeamName,
                    PositionTitle = y.Key.PositionTitle,
                    Roles = y.Select(s => new IGenericLookup()
                    {
                        value = s.RoleId,
                        label = s.RoleName,
                        checkStatus = s.RoleStatus
                    }
                    ).ToList()
                });
            Log.Information("GetEmployeeRoleMapping Service:Completed");
            return grouped;
        }

        public async Task<RmEmployeeRoleMappingDTO> AddOrUpdateEmployeeRoleMapping(RmEmployeeRoleMappingDTO mappingEntity)
        {
            Log.Information("AddOrUpdateEmployeeRoleMapping Service:Started");
            var alreadyExistsEntity = _dbContext.RmEmployeeRoleMappings.FirstOrDefault(f => f.RoleId == mappingEntity.RoleId && f.EmployeeEmail == mappingEntity.EmployeeEmail);
            if (alreadyExistsEntity != null)// Updated record in db 
            {
                alreadyExistsEntity.ModifiedAt = System.DateTime.Now;
                alreadyExistsEntity.IsActive = mappingEntity.IsActive;
                _dbContext.Update(alreadyExistsEntity);
            }
            else// Add new in DB
            {
                RmEmployeeRoleMapping newEntity = new();
                mappingEntity.CreatedAt = System.DateTime.Now;
                mappingEntity.IsActive = true;
                newEntity = _mapper.Map<RmEmployeeRoleMappingDTO, RmEmployeeRoleMapping>(mappingEntity);
                _dbContext.RmEmployeeRoleMappings.Add(newEntity);
            }
            await _dbContext.SaveChangesAsync();
            Log.Information("AddOrUpdateEmployeeRoleMapping Service:Completed");
            return mappingEntity;
        }

    }
}
