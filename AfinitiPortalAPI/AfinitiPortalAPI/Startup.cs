using AfinitiPortalAPI.ActionFilters;
using AfinitiPortalAPI.Data.PortalDBContext;
using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared.ApiClient;
using AfinitiPortalAPI.Shared.Authorization;
using AfinitiPortalAPI.Shared.Crypto;
using AfinitiPortalAPI.Shared.JsonConverters;
using AfinitiPortalAPI.Shared.Shared;
using AfinitiPortalAPI.Shared.Shared.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace AfinitiPortalAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // In-Memory Cache...
            services.AddMemoryCache();

            // Configure AutoMapper...
            services.AddAutoMapper(typeof(Startup));

            // Configure App Settings...
            var AppSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(AppSettingsSection);
            var Settings = AppSettingsSection.Get<AppSettings>();

            //Add CORS...
            services.AddCors(options =>
            {
                options.AddPolicy("Policy1",
                    builder =>
                    {
                        builder.WithOrigins(Settings.Portal_Client_URL)
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                        // TODO If cookie authenticaation enables then .AllowCredentials()
                    });
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HandleException)); // Global Exception Handling
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.Converters.Add(new CustomDecimalConverter());
            });
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AfinitiPortalAPI", Version = "v1" });
            });

            //Configure DB Context...
            var plainConnStr = CryptoUtils.AES.Decrypt(Settings.DBConnectionString);
            services.AddDbContext<PortalDBContext>(options => options.UseMySql(plainConnStr, ServerVersion.AutoDetect(plainConnStr)));

            //Add named Http Clients
            services.AddHttpClient(nameof(UserService));
            services.AddHttpContextAccessor();

            // Add WebApiContext...
            services.AddScoped<WebApiContext>();

            //Add Transient Services
            services.AddTransient<IUserService, UserService>(
                sp =>
                {
                    var htpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                    var UserServiceHttpClient = htpClientFactory.CreateClient(nameof(UserService));
                    var appSettings = sp.GetService<IOptions<AppSettings>>();
                    var dbContext = sp.GetService<PortalDBContext>();
                    var contextAccessor = sp.GetService<IHttpContextAccessor>();
                    var cacheProvider = sp.GetService<IMemoryCache>();
                    var webApiContext = sp.GetService<WebApiContext>();
                    return new UserService(appSettings, UserServiceHttpClient, dbContext, contextAccessor, cacheProvider, webApiContext);
                });

            services.AddTransient<IOrganizationChartService, OrganizationChartService>(
                sp =>
                {
                    var dbContext = sp.GetService<PortalDBContext>();
                    var mapper = sp.GetService<IMapper>();
                    var cacheProvider = sp.GetService<IMemoryCache>();
                    var appSettings = sp.GetService<IOptions<AppSettings>>();
                    var webApiContext = sp.GetService<WebApiContext>();
                    return new OrganizationChartService(dbContext, mapper, cacheProvider, appSettings, webApiContext);
                });

            services.AddScoped<IAcctRegionMappingService, AcctRegionMappingService>();
            services.AddScoped<IEmployeeRegionMappingService, EmployeeRegionMappingService>();
            services.AddScoped<IEmployeeRoleMappingService, EmployeeRoleMappingService>();
            services.AddScoped<IKPIDashboardService, KPIDashboardService>();
            services.AddScoped<ILookupsService, LookupsService>();

            services.AddSingleton<PermissionFactory>();

            // TrackerApi...
            services.AddScoped<SendToTrackerApi>();
            services.AddScoped<TrackerApiClient>();

        }

        private NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            // JSON Patch support in aspnet core 3.0 still has JSON.Net dependency - https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.0
            var jsonPatchServices = new ServiceCollection();
            jsonPatchServices.AddLogging().AddMvc().AddNewtonsoftJson();
            var jsonPatchServiceProvider = jsonPatchServices.BuildServiceProvider();
            var jsonPatchOptions = jsonPatchServiceProvider.GetRequiredService<IOptions<MvcOptions>>().Value;
            return jsonPatchOptions.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AfinitiPortalAPI v1"));
            }

            app.UseHttpsRedirection();

            //Serilog
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors("Policy1");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
