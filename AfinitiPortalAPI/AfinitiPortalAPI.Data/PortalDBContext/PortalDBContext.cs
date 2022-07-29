using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AfinitiPortalAPI.Data.PortalDBContext.Entity;

#nullable disable

namespace AfinitiPortalAPI.Data.PortalDBContext
{
    public partial class PortalDBContext : DbContext
    {
        public PortalDBContext()
        {
        }

        public PortalDBContext(DbContextOptions<PortalDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HermesAccountPhase> HermesAccountPhases { get; set; }
        public virtual DbSet<HermesCountry> HermesCountries { get; set; }
        public virtual DbSet<HermesEnumMember> HermesEnumMembers { get; set; }
        public virtual DbSet<HermesOrganization> HermesOrganizations { get; set; }
        public virtual DbSet<HermesQueue> HermesQueues { get; set; }
        public virtual DbSet<HermesQueueSwitchPlatform> HermesQueueSwitchPlatforms { get; set; }
        public virtual DbSet<HermesRegion> HermesRegions { get; set; }
        public virtual DbSet<HermesSwitchPlatform> HermesSwitchPlatforms { get; set; }
        public virtual DbSet<KpiBackendPortal> KpiBackendPortals { get; set; }
        public virtual DbSet<KpiTargetScore> KpiTargetScores { get; set; }
        public virtual DbSet<PaycomEmployeeFull> PaycomEmployeeFulls { get; set; }
        public virtual DbSet<PortalOkrBackend> PortalOkrBackends { get; set; }
        public virtual DbSet<Portaluser> Portalusers { get; set; }
        public virtual DbSet<ResourceModelRolesGroup> ResourceModelRolesGroups { get; set; }
        public virtual DbSet<ResourceModelRolesLookup> ResourceModelRolesLookups { get; set; }
        public virtual DbSet<RmAccountRegionalMapping> RmAccountRegionalMappings { get; set; }
        public virtual DbSet<RmApprovalWorkflow> RmApprovalWorkflows { get; set; }
        public virtual DbSet<RmDefinitionAssignment> RmDefinitionAssignments { get; set; }
        public virtual DbSet<RmDepartment> RmDepartments { get; set; }
        public virtual DbSet<RmEmployeeRegionalMapping> RmEmployeeRegionalMappings { get; set; }
        public virtual DbSet<RmEmployeeRoleMapping> RmEmployeeRoleMappings { get; set; }
        public virtual DbSet<RmEmployeeSwitchPlatformKnowledge> RmEmployeeSwitchPlatformKnowledges { get; set; }
        public virtual DbSet<RmPhase> RmPhases { get; set; }
        public virtual DbSet<RmPlanning> RmPlannings { get; set; }
        public virtual DbSet<RmProduct> RmProducts { get; set; }
        public virtual DbSet<RmRegion> RmRegions { get; set; }
        public virtual DbSet<RmResourceModelDefinition> RmResourceModelDefinitions { get; set; }
        public virtual DbSet<RmRevenueBracket> RmRevenueBrackets { get; set; }
        public virtual DbSet<RmRole> RmRoles { get; set; }
        public virtual DbSet<RmSubdepartment> RmSubdepartments { get; set; }
        public virtual DbSet<RmTeam> RmTeams { get; set; }
        public virtual DbSet<ServiceCatalogueLookup> ServiceCatalogueLookups { get; set; }
        public virtual DbSet<ServiceCatalogueRole> ServiceCatalogueRoles { get; set; }
        public virtual DbSet<SidRpWorkflow1> SidRpWorkflow1s { get; set; }
        public virtual DbSet<TsJiraIssueCategory> TsJiraIssueCategories { get; set; }
        public virtual DbSet<TsJiraIssueType> TsJiraIssueTypes { get; set; }
        public virtual DbSet<TsPeriod> TsPeriods { get; set; }
        public virtual DbSet<TsWorklog> TsWorklogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=10.32.21.165;port=3004;database=portaldb;uid=akay;pwd=P@ssw0rd;allowloadlocalinfile=true;treattinyasboolean=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.26-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<HermesAccountPhase>(entity =>
            {
                entity.ToTable("hermes_account_phases");

                entity.HasIndex(e => e.Value, "Value_Idx");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Enum).HasMaxLength(450);
            });

            modelBuilder.Entity<HermesCountry>(entity =>
            {
                entity.ToTable("hermes_countries");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(450);

                entity.Property(e => e.Flag).HasColumnType("text");

                entity.Property(e => e.Name).HasColumnType("text");
            });

            modelBuilder.Entity<HermesEnumMember>(entity =>
            {
                entity.ToTable("hermes_enum_members");

                entity.HasIndex(e => e.Value, "Value_Idx");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Enum).HasMaxLength(450);
            });

            modelBuilder.Entity<HermesOrganization>(entity =>
            {
                entity.ToTable("hermes_organizations");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AccentureFirstCalMeetingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Accenture_FirstCalMeetingDate");

                entity.Property(e => e.AccentureFirstJointMeetingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Accenture_FirstJointMeetingDate");

                entity.Property(e => e.AllQueuesDeployed).HasColumnType("bit(1)");

                entity.Property(e => e.Cc1Exempted).HasColumnType("bit(1)");

                entity.Property(e => e.ClientAgreedToExpand).HasColumnType("bit(1)");

                entity.Property(e => e.Code).HasMaxLength(450);

                entity.Property(e => e.DataCanBeUsedInPresentations).HasColumnType("bit(1)");

                entity.Property(e => e.Deleted).HasColumnType("bit(1)");

                entity.Property(e => e.Discriminator)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.EnterpriseDealMinimumFee).HasColumnType("bit(1)");

                entity.Property(e => e.FactSetId).HasColumnType("text");

                entity.Property(e => e.Inactive).HasColumnType("bit(1)");

                entity.Property(e => e.IndexKey).HasColumnType("text");

                entity.Property(e => e.IsAiroTarget).HasColumnType("bit(1)");

                entity.Property(e => e.Name).HasColumnType("text");
            });

            modelBuilder.Entity<HermesQueue>(entity =>
            {
                entity.ToTable("hermes_queues");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FullDataAccessDetails).HasColumnType("text");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.OptimizationDescriptionDetails).HasColumnType("text");

                entity.Property(e => e.PilotStrategicRationale).HasColumnType("text");
            });

            modelBuilder.Entity<HermesQueueSwitchPlatform>(entity =>
            {
                entity.ToTable("hermes_queue_switch_platforms");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<HermesRegion>(entity =>
            {
                entity.ToTable("hermes_regions");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnType("text");
            });

            modelBuilder.Entity<HermesSwitchPlatform>(entity =>
            {
                entity.ToTable("hermes_switch_platforms");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasColumnType("text");
            });

            modelBuilder.Entity<KpiBackendPortal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("kpi_backend_portal");

                entity.Property(e => e.DeliverySpeedIntervalSe).HasColumnName("Delivery_Speed_Interval_SE");

                entity.Property(e => e.DeliverySpeedIntervalSeOld).HasColumnName("Delivery_Speed_Interval_SE_Old");

                entity.Property(e => e.DeliverySpeedIntervalSg).HasColumnName("Delivery_Speed_Interval_SG");

                entity.Property(e => e.DeliverySpeedIntervalSgOld).HasColumnName("Delivery_Speed_Interval_SG_Old");

                entity.Property(e => e.DeliverySpeedPoL6wAvg).HasColumnName("Delivery_Speed_PO_L6W_Avg");

                entity.Property(e => e.DeliverySpeedPoL6wAvgOld).HasColumnName("Delivery_Speed_PO_L6W_Avg_Old");

                entity.Property(e => e.DeliverySpeedSgL6wAvg).HasColumnName("Delivery_Speed_SG_L6W_Avg");

                entity.Property(e => e.DeliverySpeedSgL6wAvgOld).HasColumnName("Delivery_Speed_SG_L6W_Avg_Old");

                entity.Property(e => e.DeliverySpeedSpIntervalSe).HasColumnName("Delivery_Speed_SP_Interval_SE");

                entity.Property(e => e.DeliverySpeedSpIntervalSg).HasColumnName("Delivery_Speed_SP_Interval_SG");

                entity.Property(e => e.DeliverySpeedSpPoL6wAvg).HasColumnName("Delivery_Speed_SP_PO_L6W_Avg");

                entity.Property(e => e.DeliverySpeedSpSgL6wAvg).HasColumnName("Delivery_Speed_SP_SG_L6W_Avg");

                entity.Property(e => e.EndOfWeekDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_of_week_date");

                entity.Property(e => e.Interval).HasColumnType("text");

                entity.Property(e => e.Portfolio)
                    .HasColumnType("text")
                    .HasColumnName("PORTFOLIO");

                entity.Property(e => e.ProdQualityIntervalSe).HasColumnName("Prod_Quality_Interval_SE");

                entity.Property(e => e.ProdQualityIntervalSeOld).HasColumnName("Prod_Quality_Interval_SE_Old");

                entity.Property(e => e.ProdQualityIntervalSg).HasColumnName("Prod_Quality_Interval_SG");

                entity.Property(e => e.ProdQualityIntervalSgOld).HasColumnName("Prod_Quality_Interval_SG_Old");

                entity.Property(e => e.ProdQualityPoL6wAvg).HasColumnName("Prod_Quality_PO_L6W_Avg");

                entity.Property(e => e.ProdQualityPoL6wAvgOld).HasColumnName("Prod_Quality_PO_L6W_Avg_Old");

                entity.Property(e => e.ProdQualitySgL6wAvg).HasColumnName("Prod_Quality_SG_L6W_Avg");

                entity.Property(e => e.ProdQualitySgL6wAvgOld).HasColumnName("Prod_Quality_SG_L6W_Avg_Old");

                entity.Property(e => e.QaLeadTimeIntervalSe).HasColumnName("QA_Lead_Time_Interval_SE");

                entity.Property(e => e.QaLeadTimeIntervalSg).HasColumnName("QA_Lead_Time_Interval_SG");

                entity.Property(e => e.QaLeadTimePoL6wAvg).HasColumnName("QA_Lead_Time_PO_L6W_Avg");

                entity.Property(e => e.QaLeadTimeSgL6wAvg).HasColumnName("QA_Lead_Time_SG_L6W_Avg");

                entity.Property(e => e.QualityIntervalSe).HasColumnName("Quality_Interval_SE");

                entity.Property(e => e.QualityIntervalSg).HasColumnName("Quality_Interval_SG");

                entity.Property(e => e.QualityPoL6wAvg).HasColumnName("Quality_PO_L6W_Avg");

                entity.Property(e => e.QualitySgL6wAvg).HasColumnName("Quality_SG_L6W_Avg");

                entity.Property(e => e.Service)
                    .HasColumnType("text")
                    .HasColumnName("SERVICE");

                entity.Property(e => e.ServiceGroup)
                    .HasColumnType("text")
                    .HasColumnName("SERVICE_GROUP");

                entity.Property(e => e.StartOfWeekDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_of_week_date");
            });

            modelBuilder.Entity<KpiTargetScore>(entity =>
            {
                entity.ToTable("kpi_target_scores");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeliverySpeed).HasColumnName("delivery_speed");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.ProductionQuality).HasColumnName("production_quality");

                entity.Property(e => e.Quality).HasColumnName("quality");

                entity.Property(e => e.Quarter)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("quarter");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");
            });

            modelBuilder.Entity<PaycomEmployeeFull>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("paycom_employee_full");

                entity.Property(e => e.ActualMaritalStatus)
                    .HasMaxLength(250)
                    .HasColumnName("actual_marital_status");

                entity.Property(e => e.ActualMaritalStatusDescription)
                    .HasMaxLength(250)
                    .HasColumnName("actual_marital_status_description");

                entity.Property(e => e.AfinitiAge)
                    .HasPrecision(10)
                    .HasColumnName("afiniti_age");

                entity.Property(e => e.AfinitiTotalAge)
                    .HasPrecision(10)
                    .HasColumnName("afiniti_total_age");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.AllWorkStatus)
                    .HasMaxLength(250)
                    .HasColumnName("ALL_WORK_STATUS");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("birth_date");

                entity.Property(e => e.BusinessTitle)
                    .HasMaxLength(250)
                    .HasColumnName("business_title");

                entity.Property(e => e.Ceo)
                    .HasColumnType("text")
                    .HasColumnName("CEO");

                entity.Property(e => e.CeoCode)
                    .HasColumnType("text")
                    .HasColumnName("CEO_CODE");

                entity.Property(e => e.CeoDre).HasColumnName("CEO_DRE");

                entity.Property(e => e.ChangeStatus)
                    .HasMaxLength(14)
                    .HasColumnName("CHANGE_STATUS");

                entity.Property(e => e.Changetime)
                    .HasColumnType("datetime")
                    .HasColumnName("changetime");

                entity.Property(e => e.City)
                    .HasMaxLength(250)
                    .HasColumnName("CITY");

                entity.Property(e => e.Clocksequencenumber)
                    .HasMaxLength(250)
                    .HasColumnName("clocksequencenumber")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CompanyLocationId).HasColumnName("companyLocationId");

                entity.Property(e => e.Country).HasColumnType("text");

                entity.Property(e => e.CurrentWorkinFlag).HasColumnName("CURRENT_WORKIN_FLAG");

                entity.Property(e => e.Department).HasMaxLength(250);

                entity.Property(e => e.DepartmentCode)
                    .HasMaxLength(250)
                    .HasColumnName("department_code");

                entity.Property(e => e.DirectReports).HasColumnName("Direct Reports");

                entity.Property(e => e.DolStatus)
                    .HasColumnType("text")
                    .HasColumnName("dol_status");

                entity.Property(e => e.EighthSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("EIGHTH_SUPERVISOR");

                entity.Property(e => e.EighthSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("EIGHTH_SUPERVISOR_CODE");

                entity.Property(e => e.EigthLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("EIGTH_LEVEL_BELOW_CEO");

                entity.Property(e => e.EigthLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("EIGTH_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.EigthLevelBelowCeoCodeDre).HasColumnName("EIGTH_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.EmployeeAdded)
                    .HasColumnType("datetime")
                    .HasColumnName("employee_added");

                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("employee_code");

                entity.Property(e => e.EmployeeGlCode)
                    .HasMaxLength(250)
                    .HasColumnName("employee_gl_code");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(250)
                    .HasColumnName("employee_name");

                entity.Property(e => e.EmployeeStatus)
                    .HasColumnType("text")
                    .HasColumnName("employee_status");

                entity.Property(e => e.EmployeeType1099).HasColumnName("employee_type_1099");

                entity.Property(e => e.EmploymentType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Employment_Type");

                entity.Property(e => e.EthnicBackground)
                    .HasColumnType("text")
                    .HasColumnName("ethnic_background");

                entity.Property(e => e.EtlDate)
                    .HasColumnType("datetime")
                    .HasColumnName("etl_date");

                entity.Property(e => e.ExemptStatus)
                    .HasColumnType("text")
                    .HasColumnName("exempt_status");

                entity.Property(e => e.FifthSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("FIFTH_SUPERVISOR");

                entity.Property(e => e.FifthSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("FIFTH_SUPERVISOR_CODE");

                entity.Property(e => e.FirstHireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("First_Hire_Date");

                entity.Property(e => e.FirstSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("FIRST_SUPERVISOR");

                entity.Property(e => e.FirstSupervisorCheck)
                    .HasColumnType("text")
                    .HasColumnName("FIRST_SUPERVISOR_CHECK");

                entity.Property(e => e.FirstSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("FIRST_SUPERVISOR_CODE");

                entity.Property(e => e.FirstSupervisorCodeCheck)
                    .HasMaxLength(250)
                    .HasColumnName("FIRST_SUPERVISOR_CODE_CHECK");

                entity.Property(e => e.FirstSupervisorDepartment)
                    .HasMaxLength(250)
                    .HasColumnName("first_supervisor_department");

                entity.Property(e => e.FirstSupervisorWorkEmail)
                    .HasMaxLength(250)
                    .HasColumnName("first_supervisor_work_email");

                entity.Property(e => e.FirstSupervisorWorkStatus)
                    .HasMaxLength(250)
                    .HasColumnName("FIRST_SUPERVISOR_WORK_STATUS");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(250)
                    .HasColumnName("firstname");

                entity.Property(e => e.FiveLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("FIVE_LEVEL_BELOW_CEO");

                entity.Property(e => e.FiveLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("FIVE_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.FiveLevelBelowCeoCodeDre).HasColumnName("FIVE_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.FourLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("FOUR_LEVEL_BELOW_CEO");

                entity.Property(e => e.FourLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("FOUR_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.FourLevelBelowCeoCodeDre).HasColumnName("FOUR_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.FourthSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("FOURTH_SUPERVISOR");

                entity.Property(e => e.FourthSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("FOURTH_SUPERVISOR_CODE");

                entity.Property(e => e.FullName)
                    .HasColumnType("text")
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.FulltimeOrParttime).HasColumnName("fulltime_or_parttime");

                entity.Property(e => e.Gender)
                    .HasMaxLength(250)
                    .HasColumnName("gender")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.GenderDesc)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasColumnName("gender_desc");

                entity.Property(e => e.HasDirectDeposit).HasColumnName("has_direct_deposit");

                entity.Property(e => e.HireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("hire_date");

                entity.Property(e => e.HireDateL12mFlag).HasColumnName("HIRE_DATE_L12M_FLAG");

                entity.Property(e => e.HireMonth)
                    .HasColumnType("text")
                    .HasColumnName("Hire_Month");

                entity.Property(e => e.HireRank).HasColumnName("Hire_Rank");

                entity.Property(e => e.HourlyOrSalary).HasColumnName("hourly_or_salary");

                entity.Property(e => e.LaborAllocationDetails)
                    .HasMaxLength(250)
                    .HasColumnName("labor_allocation_details");

                entity.Property(e => e.LastPayChange)
                    .HasColumnType("datetime")
                    .HasColumnName("last_pay_change");

                entity.Property(e => e.LastPositionChangeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_position_change_date");

                entity.Property(e => e.LastReview)
                    .HasColumnType("datetime")
                    .HasColumnName("last_review");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(250)
                    .HasColumnName("lastname");

                entity.Property(e => e.LegalCountry)
                    .HasColumnType("text")
                    .HasColumnName("LEGAL_COUNTRY");

                entity.Property(e => e.LegalEntity)
                    .HasColumnType("text")
                    .HasColumnName("LEGAL_ENTITY");

                entity.Property(e => e.LengthOfServiceSinceRehire)
                    .HasMaxLength(250)
                    .HasColumnName("length_of_service_since_rehire");

                entity.Property(e => e.Level)
                    .HasMaxLength(3)
                    .HasColumnName("LEVEL");

                entity.Property(e => e.LocationRaw)
                    .HasColumnType("text")
                    .HasColumnName("LOCATION_RAW");

                entity.Property(e => e.MaxDay).HasColumnName("MAX_DAY");

                entity.Property(e => e.Middlename)
                    .HasMaxLength(250)
                    .HasColumnName("middlename");

                entity.Property(e => e.MinDay).HasColumnName("MIN_DAY");

                entity.Property(e => e.MostRecentCheckDate)
                    .HasColumnType("datetime")
                    .HasColumnName("most_recent_check_date");

                entity.Property(e => e.NewHire).HasColumnName("new_hire");

                entity.Property(e => e.NewHireReportDate)
                    .HasColumnType("datetime")
                    .HasColumnName("new_hire_report_date");

                entity.Property(e => e.NextReview)
                    .HasColumnType("datetime")
                    .HasColumnName("next_review");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(250)
                    .HasColumnName("nickname");

                entity.Property(e => e.NineLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("NINE_LEVEL_BELOW_CEO");

                entity.Property(e => e.NineLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("NINE_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.NineLevelBelowCeoCodeDre).HasColumnName("NINE_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.NinthSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("NINTH_SUPERVISOR");

                entity.Property(e => e.NinthSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("NINTH_SUPERVISOR_CODE");

                entity.Property(e => e.NtLogin)
                    .HasMaxLength(250)
                    .HasColumnName("NT_Login");

                entity.Property(e => e.OneLevBelowCeoDepartment)
                    .HasMaxLength(250)
                    .HasColumnName("one_lev_below_ceo_department");

                entity.Property(e => e.OneLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("ONE_LEVEL_BELOW_CEO");

                entity.Property(e => e.OneLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("ONE_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.OneLevelBelowCeoCodeDre).HasColumnName("ONE_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.PayClass)
                    .HasMaxLength(250)
                    .HasColumnName("pay_class");

                entity.Property(e => e.PayFrequency)
                    .HasMaxLength(250)
                    .HasColumnName("pay_frequency");

                entity.Property(e => e.PeopleManager)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("PEOPLE_MANAGER");

                entity.Property(e => e.PersonalEmail)
                    .HasMaxLength(250)
                    .HasColumnName("personal_email");

                entity.Property(e => e.Portfolio).HasColumnType("text");

                entity.Property(e => e.Position)
                    .HasMaxLength(250)
                    .HasColumnName("position");

                entity.Property(e => e.PositionCode)
                    .HasMaxLength(250)
                    .HasColumnName("position_code");

                entity.Property(e => e.PositionFamily)
                    .HasMaxLength(250)
                    .HasColumnName("position_family");

                entity.Property(e => e.PositionFamilyCode)
                    .HasMaxLength(250)
                    .HasColumnName("position_family_code");

                entity.Property(e => e.PositionFamilyName)
                    .HasMaxLength(250)
                    .HasColumnName("position_family_name");

                entity.Property(e => e.PositionId)
                    .HasMaxLength(250)
                    .HasColumnName("position_id");

                entity.Property(e => e.PositionSeatNumber)
                    .HasMaxLength(250)
                    .HasColumnName("position_seat_number");

                entity.Property(e => e.PositionSeatTitle)
                    .HasMaxLength(250)
                    .HasColumnName("position_seat_title");

                entity.Property(e => e.PositionTitle)
                    .HasMaxLength(250)
                    .HasColumnName("position_title");

                entity.Property(e => e.PreviousTerminationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("previous_termination_date");

                entity.Property(e => e.PrimaryPhone)
                    .HasMaxLength(250)
                    .HasColumnName("primary_phone");

                entity.Property(e => e.PrimaryPhoneType).HasColumnName("primary_phone_type");

                entity.Property(e => e.RawLegalEntity)
                    .HasMaxLength(250)
                    .HasColumnName("RAW_LEGAL_ENTITY");

                entity.Property(e => e.RedBlueTeamFlag)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("RED_BLUE_TEAM_FLAG");

                entity.Property(e => e.RehireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Rehire_Date");

                entity.Property(e => e.RehireDateRaw)
                    .HasColumnType("datetime")
                    .HasColumnName("rehire_date_raw");

                entity.Property(e => e.ReportNewHire).HasColumnName("report_new_hire");

                entity.Property(e => e.RoleName)
                    .HasColumnType("text")
                    .HasColumnName("Role_Name");

                entity.Property(e => e.SecondSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("SECOND_SUPERVISOR");

                entity.Property(e => e.SecondSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("SECOND_SUPERVISOR_CODE");

                entity.Property(e => e.SecondaryPhone)
                    .HasColumnType("text")
                    .HasColumnName("secondary_phone");

                entity.Property(e => e.SecondaryPhoneType).HasColumnName("secondary_phone_type");

                entity.Property(e => e.Service).HasColumnType("text");

                entity.Property(e => e.ServiceGroup)
                    .HasColumnType("text")
                    .HasColumnName("Service_Group");

                entity.Property(e => e.SevenLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("SEVEN_LEVEL_BELOW_CEO");

                entity.Property(e => e.SevenLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("SEVEN_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.SevenLevelBelowCeoCodeDre).HasColumnName("SEVEN_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.SeventhSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("SEVENTH_SUPERVISOR");

                entity.Property(e => e.SeventhSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("SEVENTH_SUPERVISOR_CODE");

                entity.Property(e => e.SixLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("SIX_LEVEL_BELOW_CEO");

                entity.Property(e => e.SixLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("SIX_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.SixLevelBelowCeoCodeDre).HasColumnName("SIX_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.SixthSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("SIXTH_SUPERVISOR");

                entity.Property(e => e.SixthSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("SIXTH_SUPERVISOR_CODE");

                entity.Property(e => e.SnapshotHireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("SNAPSHOT_HIRE_DATE");

                entity.Property(e => e.State)
                    .HasColumnType("text")
                    .HasColumnName("state");

                entity.Property(e => e.Street)
                    .HasColumnType("text")
                    .HasColumnName("street");

                entity.Property(e => e.SubDepartment)
                    .HasMaxLength(250)
                    .HasColumnName("Sub Department");

                entity.Property(e => e.SubDepartmentCode)
                    .HasMaxLength(250)
                    .HasColumnName("Sub Department Code")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.SubDepartmentDisplayName)
                    .HasMaxLength(250)
                    .HasColumnName("Sub_Department_Display_Name");

                entity.Property(e => e.SupervisorApproval)
                    .HasMaxLength(250)
                    .HasColumnName("supervisor_approval");

                entity.Property(e => e.SupervisorOfOneself).HasColumnName("supervisor_of_oneself");

                entity.Property(e => e.Team).HasMaxLength(250);

                entity.Property(e => e.TeamCode)
                    .HasMaxLength(250)
                    .HasColumnName("Team Code")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.TenthSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("TENTH_SUPERVISOR");

                entity.Property(e => e.TenthSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("TENTH_SUPERVISOR_CODE");

                entity.Property(e => e.TerminationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("termination_date");

                entity.Property(e => e.TerminationDateL12mFlag).HasColumnName("TERMINATION_DATE_L12M_FLAG");

                entity.Property(e => e.TerminationMonth)
                    .HasColumnType("text")
                    .HasColumnName("Termination_Month");

                entity.Property(e => e.TerminationReason)
                    .HasMaxLength(250)
                    .HasColumnName("termination_reason");

                entity.Property(e => e.ThirdSupervisor)
                    .HasColumnType("text")
                    .HasColumnName("THIRD_SUPERVISOR");

                entity.Property(e => e.ThirdSupervisorCode)
                    .HasColumnType("text")
                    .HasColumnName("THIRD_SUPERVISOR_CODE");

                entity.Property(e => e.ThreeLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("THREE_LEVEL_BELOW_CEO");

                entity.Property(e => e.ThreeLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("THREE_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.ThreeLevelBelowCeoCodeDre).HasColumnName("THREE_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.TotalHeadcount).HasColumnName("Total Headcount");

                entity.Property(e => e.TwoLevelBelowCeo)
                    .HasColumnType("text")
                    .HasColumnName("TWO_LEVEL_BELOW_CEO");

                entity.Property(e => e.TwoLevelBelowCeoCode)
                    .HasColumnType("text")
                    .HasColumnName("TWO_LEVEL_BELOW_CEO_CODE");

                entity.Property(e => e.TwoLevelBelowCeoCodeDre).HasColumnName("TWO_LEVEL_BELOW_CEO_CODE_DRE");

                entity.Property(e => e.WorkEmail)
                    .HasMaxLength(250)
                    .HasColumnName("work_email");

                entity.Property(e => e.WorkStatus)
                    .HasColumnType("text")
                    .HasColumnName("WORK_STATUS");

                entity.Property(e => e.Zipcode)
                    .HasColumnType("text")
                    .HasColumnName("zipcode");
            });

            modelBuilder.Entity<PortalOkrBackend>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("portal_okr_backend");

                entity.Property(e => e.Interval)
                    .HasMaxLength(255)
                    .HasColumnName("INTERVAL");

                entity.Property(e => e.OkrLevel)
                    .IsRequired()
                    .HasMaxLength(28)
                    .HasColumnName("OKR_LEVEL");

                entity.Property(e => e.Portfolio)
                    .HasMaxLength(255)
                    .HasColumnName("PORTFOLIO");

                entity.Property(e => e.PortfolioOkr)
                    .HasMaxLength(255)
                    .HasColumnName("Portfolio_OKR");

                entity.Property(e => e.PortfolioOkrIssueLink)
                    .HasMaxLength(255)
                    .HasColumnName("Portfolio_OKR_Issue_Link");

                entity.Property(e => e.Service).HasMaxLength(255);

                entity.Property(e => e.ServiceGroup)
                    .HasMaxLength(255)
                    .HasColumnName("Service_Group");

                entity.Property(e => e.ServiceGroupOkr)
                    .HasMaxLength(255)
                    .HasColumnName("Service_Group_OKR");

                entity.Property(e => e.ServiceGroupOkrIssueLink)
                    .HasMaxLength(255)
                    .HasColumnName("Service_Group_OKR_Issue_Link");

                entity.Property(e => e.ServiceOkr)
                    .HasMaxLength(255)
                    .HasColumnName("Service_OKR");

                entity.Property(e => e.ServiceOkrIssueLink)
                    .HasMaxLength(255)
                    .HasColumnName("Service_OKR_Issue_Link");
            });

            modelBuilder.Entity<Portaluser>(entity =>
            {
                entity.ToTable("portalusers");

                entity.Property(e => e.ApprovalStatus)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(250)
                    .HasColumnName("Employee_Code");

                entity.Property(e => e.IsActive).HasDefaultValueSql("'0'");

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("'0'");

                entity.Property(e => e.LastSrikeDate)
                    .HasColumnType("datetime")
                    .HasColumnName("LastSrike_Date");

                entity.Property(e => e.StrikeCount).HasDefaultValueSql("'0'");

                entity.Property(e => e.UserName).HasMaxLength(250);
            });

            modelBuilder.Entity<ResourceModelRolesGroup>(entity =>
            {
                entity.ToTable("resource_model_roles_groups");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InsertionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.RoleGroupId).HasColumnName("Role_Group_Id");

                entity.Property(e => e.RoleGroupName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("Role_Group_Name");
            });

            modelBuilder.Entity<ResourceModelRolesLookup>(entity =>
            {
                entity.ToTable("resource_model_roles_lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InsertionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive).HasDefaultValueSql("'0'");

                entity.Property(e => e.IsDedicated).HasDefaultValueSql("'0'");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.PaycomSubDepartment)
                    .HasMaxLength(100)
                    .HasColumnName("Paycom_Sub_Department");

                entity.Property(e => e.RoleGroupId).HasColumnName("Role_Group_Id");

                entity.Property(e => e.RoleResourceModel)
                    .HasMaxLength(255)
                    .HasColumnName("Role_Resource_Model");

                entity.Property(e => e.Shifts).HasDefaultValueSql("'0'");

                entity.Property(e => e.Status).HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<RmAccountRegionalMapping>(entity =>
            {
                entity.ToTable("rm_account_regional_mapping");

                entity.HasIndex(e => e.AccountId, "fk_arm_account_id");

                entity.HasIndex(e => e.RegionId, "fk_arm_region_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.RmAccountRegionalMappings)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("fk_arm_region_id");
            });

            modelBuilder.Entity<RmApprovalWorkflow>(entity =>
            {
                entity.ToTable("rm_approval_workflow");

                entity.HasIndex(e => new { e.DepartmentId, e.SubdepartmentId, e.TeamId, e.WorkflowOrder, e.ApproverEmail }, "aw_ri_wo_ae_uidx")
                    .IsUnique();

                entity.HasIndex(e => e.SubdepartmentId, "fk_aw_subdepartment_id");

                entity.HasIndex(e => e.TeamId, "fk_aw_team_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApproverEmail)
                    .IsRequired()
                    .HasColumnName("approver_email");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.SubdepartmentId).HasColumnName("subdepartment_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.Property(e => e.WorkflowOrder).HasColumnName("workflow_order");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.RmApprovalWorkflows)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("fk_aw_department_id");

                entity.HasOne(d => d.Subdepartment)
                    .WithMany(p => p.RmApprovalWorkflows)
                    .HasForeignKey(d => d.SubdepartmentId)
                    .HasConstraintName("fk_aw_subdepartment_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.RmApprovalWorkflows)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("fk_aw_team_id");
            });

            modelBuilder.Entity<RmDefinitionAssignment>(entity =>
            {
                entity.ToTable("rm_definition_assignments");

                entity.HasIndex(e => e.DefinitionId, "fk_def_asg_definition_id");

                entity.HasIndex(e => e.ProductId, "fk_def_asg_product_id");

                entity.HasIndex(e => e.RevenueBracketId, "fk_def_asg_revenue_bracket_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DedicatedAssignment).HasColumnName("dedicated_assignment");

                entity.Property(e => e.DefinitionId).HasColumnName("definition_id");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.RevenueBracketId).HasColumnName("revenue_bracket_id");

                entity.Property(e => e.SharedAssignment).HasColumnName("shared_assignment");

                entity.HasOne(d => d.Definition)
                    .WithMany(p => p.RmDefinitionAssignments)
                    .HasForeignKey(d => d.DefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_def_asg_definition_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RmDefinitionAssignments)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_def_asg_product_id");

                entity.HasOne(d => d.RevenueBracket)
                    .WithMany(p => p.RmDefinitionAssignments)
                    .HasForeignKey(d => d.RevenueBracketId)
                    .HasConstraintName("fk_def_asg_revenue_bracket_id");
            });

            modelBuilder.Entity<RmDepartment>(entity =>
            {
                entity.ToTable("rm_departments");

                entity.HasIndex(e => e.Name, "d_name_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.PaycomDepartmentId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("paycom_department_id");

                entity.Property(e => e.PaycomDepartmentName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("paycom_department_name");
            });

            modelBuilder.Entity<RmEmployeeRegionalMapping>(entity =>
            {
                entity.ToTable("rm_employee_regional_mapping");

                entity.HasIndex(e => e.RegionId, "fk_emp_reg_map_region_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.EmployeeEmail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employee_email");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.RmEmployeeRegionalMappings)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_emp_reg_map_region_id");
            });

            modelBuilder.Entity<RmEmployeeRoleMapping>(entity =>
            {
                entity.ToTable("rm_employee_role_mapping");

                entity.HasIndex(e => e.RoleId, "fk_erm_reg_map_region_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.EmployeeEmail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employee_email");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RmEmployeeRoleMappings)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_erm_reg_map_region_id");
            });

            modelBuilder.Entity<RmEmployeeSwitchPlatformKnowledge>(entity =>
            {
                entity.ToTable("rm_employee_switch_platform_knowledge");

                entity.HasIndex(e => e.SwitchPlatformId, "fk_switch_platform_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.EmployeeEmail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employee_email");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.SwitchPlatformId).HasColumnName("switch_platform_id");
            });

            modelBuilder.Entity<RmPhase>(entity =>
            {
                entity.ToTable("rm_phases");

                entity.HasIndex(e => e.DepartmentId, "fk_p_department_id");

                entity.HasIndex(e => e.HermesPhaseId, "fk_p_hermes_phase_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.HermesPhaseId).HasColumnName("hermes_phase_id");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.RmPhases)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_p_department_id");
            });

            modelBuilder.Entity<RmPlanning>(entity =>
            {
                entity.ToTable("rm_planning");

                entity.HasIndex(e => e.AssignedAccountId, "fk_pln_assigned_queue_id");

                entity.HasIndex(e => e.AssignedRoleId, "fk_pln_assigned_role_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApprovalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approval_date");

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(255)
                    .HasColumnName("approved_by");

                entity.Property(e => e.AssignedAccountId).HasColumnName("assigned_account_id");

                entity.Property(e => e.AssignedQueueId).HasColumnName("assigned_queue_id");

                entity.Property(e => e.AssignedRoleId).HasColumnName("assigned_role_id");

                entity.Property(e => e.Assignment).HasColumnName("assignment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DataEntryBy)
                    .HasMaxLength(255)
                    .HasColumnName("data_entry_by");

                entity.Property(e => e.EmployeeEmail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employee_email");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.PlanningCycle)
                    .HasMaxLength(255)
                    .HasColumnName("planning_cycle");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.AssignedRole)
                    .WithMany(p => p.RmPlannings)
                    .HasForeignKey(d => d.AssignedRoleId)
                    .HasConstraintName("fk_pln_assigned_role_id");
            });

            modelBuilder.Entity<RmProduct>(entity =>
            {
                entity.ToTable("rm_products");

                entity.HasIndex(e => e.Name, "prod_name_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RmRegion>(entity =>
            {
                entity.ToTable("rm_regions");

                entity.HasIndex(e => e.SubdepartmentId, "fk_reg_subdepartment_id");

                entity.HasIndex(e => e.TeamId, "fk_reg_team_id");

                entity.HasIndex(e => new { e.DepartmentId, e.SubdepartmentId, e.TeamId, e.Name }, "reg_dept_subdept_team_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsImmutable).HasColumnName("is_immutable");

                entity.Property(e => e.IsRegion).HasColumnName("is_region");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.RegionalManager)
                    .HasMaxLength(255)
                    .HasColumnName("regional_manager");

                entity.Property(e => e.SubdepartmentId).HasColumnName("subdepartment_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.RmRegions)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("fk_reg_department_id");

                entity.HasOne(d => d.Subdepartment)
                    .WithMany(p => p.RmRegions)
                    .HasForeignKey(d => d.SubdepartmentId)
                    .HasConstraintName("fk_reg_subdepartment_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.RmRegions)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("fk_reg_team_id");
            });

            modelBuilder.Entity<RmResourceModelDefinition>(entity =>
            {
                entity.ToTable("rm_resource_model_definitions");

                entity.HasIndex(e => e.DepartmentId, "fk_def_department_id");

                entity.HasIndex(e => e.PhaseId, "fk_def_phase_id");

                entity.HasIndex(e => e.ProductId, "fk_def_product_id");

                entity.HasIndex(e => e.RevenueBracketId, "fk_def_revenue_bracket_id");

                entity.HasIndex(e => e.RoleId, "fk_def_role_id");

                entity.HasIndex(e => e.SubdepartmentId, "fk_def_subdepartment_id");

                entity.HasIndex(e => e.TeamId, "fk_def_team_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.IsLatestVersion).HasColumnName("is_latest_version");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.PhaseId).HasColumnName("phase_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.RevenueBracketId).HasColumnName("revenue_bracket_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SubdepartmentId).HasColumnName("subdepartment_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.Property(e => e.VersionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("version_date");

                entity.Property(e => e.VersionNo).HasColumnName("version_no");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.RmResourceModelDefinitions)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_def_department_id");

                entity.HasOne(d => d.Phase)
                    .WithMany(p => p.RmResourceModelDefinitions)
                    .HasForeignKey(d => d.PhaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_def_phase_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RmResourceModelDefinitions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_def_product_id");

                entity.HasOne(d => d.RevenueBracket)
                    .WithMany(p => p.RmResourceModelDefinitions)
                    .HasForeignKey(d => d.RevenueBracketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_def_revenue_bracket_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RmResourceModelDefinitions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_def_role_id");

                entity.HasOne(d => d.Subdepartment)
                    .WithMany(p => p.RmResourceModelDefinitions)
                    .HasForeignKey(d => d.SubdepartmentId)
                    .HasConstraintName("fk_def_subdepartment_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.RmResourceModelDefinitions)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("fk_def_team_id");
            });

            modelBuilder.Entity<RmRevenueBracket>(entity =>
            {
                entity.ToTable("rm_revenue_brackets");

                entity.HasIndex(e => e.Name, "rb_name_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Maximum)
                    .HasPrecision(11, 2)
                    .HasColumnName("maximum");

                entity.Property(e => e.Minimum)
                    .HasPrecision(11, 2)
                    .HasColumnName("minimum");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RmRole>(entity =>
            {
                entity.ToTable("rm_roles");

                entity.HasIndex(e => e.DepartmentId, "fk_roles_dpt_id");

                entity.HasIndex(e => e.SubdepartmentId, "fk_roles_subdpt_id");

                entity.HasIndex(e => e.TeamId, "fk_roles_team_id");

                entity.HasIndex(e => e.Name, "role_name_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.ShiftCount).HasColumnName("shift_count");

                entity.Property(e => e.SubdepartmentId).HasColumnName("subdepartment_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.RmRoles)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_roles_dpt_id");

                entity.HasOne(d => d.Subdepartment)
                    .WithMany(p => p.RmRoles)
                    .HasForeignKey(d => d.SubdepartmentId)
                    .HasConstraintName("fk_roles_subdpt_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.RmRoles)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("fk_roles_team_id");
            });

            modelBuilder.Entity<RmSubdepartment>(entity =>
            {
                entity.ToTable("rm_subdepartments");

                entity.HasIndex(e => new { e.DepartmentId, e.Name }, "subd_department_id_name_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.PaycomSubdepartmentId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("paycom_subdepartment_id");

                entity.Property(e => e.PaycomSubdepartmentName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("paycom_subdepartment_name");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.RmSubdepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_subd_subdepartment_id");
            });

            modelBuilder.Entity<RmTeam>(entity =>
            {
                entity.ToTable("rm_teams");

                entity.HasIndex(e => new { e.SubdepartmentId, e.Name }, "team_subdepartment_id_name_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.SubdepartmentId).HasColumnName("subdepartment_id");

                entity.HasOne(d => d.Subdepartment)
                    .WithMany(p => p.RmTeams)
                    .HasForeignKey(d => d.SubdepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_team_subdepartment_id");
            });

            modelBuilder.Entity<ServiceCatalogueLookup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("service_catalogue_lookup");

                entity.Property(e => e.Portfolio).HasMaxLength(255);

                entity.Property(e => e.PortfolioUrl)
                    .IsRequired()
                    .HasMaxLength(79)
                    .HasColumnName("Portfolio_URL");

                entity.Property(e => e.Service).HasMaxLength(255);

                entity.Property(e => e.ServiceGroup)
                    .HasMaxLength(255)
                    .HasColumnName("Service_Group");

                entity.Property(e => e.ServiceGroupUrl)
                    .IsRequired()
                    .HasMaxLength(79)
                    .HasColumnName("Service_Group_URL");

                entity.Property(e => e.ServiceUrl)
                    .IsRequired()
                    .HasMaxLength(79)
                    .HasColumnName("Service_URL");
            });

            modelBuilder.Entity<ServiceCatalogueRole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("service_catalogue_roles");

                entity.Property(e => e.EmailAddress)
                    .HasColumnType("text")
                    .HasColumnName("EMAIL_ADDRESS");

                entity.Property(e => e.LowerUserName)
                    .HasColumnType("text")
                    .HasColumnName("lower_user_name");

                entity.Property(e => e.Portfolio).HasColumnType("text");

                entity.Property(e => e.RoleName)
                    .HasColumnType("text")
                    .HasColumnName("ROLE_NAME");

                entity.Property(e => e.Service).HasColumnType("text");

                entity.Property(e => e.ServiceGroup)
                    .HasColumnType("text")
                    .HasColumnName("Service_Group");

                entity.Property(e => e.StaffName)
                    .HasColumnType("text")
                    .HasColumnName("STAFF_NAME");

                entity.Property(e => e.UserKey)
                    .HasColumnType("text")
                    .HasColumnName("user_key");
            });

            modelBuilder.Entity<SidRpWorkflow1>(entity =>
            {
                entity.ToTable("sid_rp_workflow1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive).HasDefaultValueSql("'0'");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.RegionManager)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TsJiraIssueCategory>(entity =>
            {
                entity.ToTable("ts_jira_issue_category");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(45);

                entity.Property(e => e.HermesAccount).HasDefaultValueSql("'0'");

                entity.Property(e => e.InsertionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TsJiraIssueType>(entity =>
            {
                entity.ToTable("ts_jira_issue_type");

                entity.HasIndex(e => e.IssueCategoryId, "fk_issue_type_categoryId_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code).HasMaxLength(45);

                entity.Property(e => e.InsertionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.HasOne(d => d.IssueCategory)
                    .WithMany(p => p.TsJiraIssueTypes)
                    .HasForeignKey(d => d.IssueCategoryId)
                    .HasConstraintName("fk_issue_type_categoryId");
            });

            modelBuilder.Entity<TsPeriod>(entity =>
            {
                entity.ToTable("ts_periods");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InsertionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LockStatus)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.ModifyUser).HasMaxLength(255);

                entity.Property(e => e.PeriodEndDate).HasColumnType("datetime");

                entity.Property(e => e.PeriodStartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TsWorklog>(entity =>
            {
                entity.ToTable("ts_worklogs");

                entity.HasIndex(e => e.CategoryId, "fk_worklogs_issue_category_id_idx");

                entity.HasIndex(e => e.TypeId, "fk_worklogs_issue_id_idx");

                entity.HasIndex(e => e.EmployeeEmail, "fk_worklogs_user_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproverEmail).HasMaxLength(255);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.EmployeeEmail).HasMaxLength(250);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.InsertionDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.InsertionUser)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ModifierName).HasMaxLength(255);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Week)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TsWorklogs)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_worklogs_issue_category_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TsWorklogs)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_worklogs_issue_type_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
