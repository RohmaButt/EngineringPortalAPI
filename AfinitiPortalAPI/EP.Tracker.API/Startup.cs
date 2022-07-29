using AfinitiPortalAPI.Shared.Crypto;
using AfinitiPortalAPI.Shared.Shared.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Tracker.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure App Settings...
            var AppSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(AppSettingsSection);
            var Settings = AppSettingsSection.Get<AppSettings>();

            //Add CORS...
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                    builder =>
                    {
                        builder.WithOrigins(Settings.Portal_Client_URL)
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                        // TODO If cookie authenticaation enables then .AllowCredentials()
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EP.Tracker.API", Version = "v1" });
            });

            ////Configure DB Context...
            //var plainConnStr = CryptoUtils.AES.Decrypt(Settings.DBConnectionString);
            //services.AddDbContext<PortalDBContext>(options => options.UseMySql(plainConnStr, ServerVersion.AutoDetect(plainConnStr)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EP.Tracker.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("DefaultPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
