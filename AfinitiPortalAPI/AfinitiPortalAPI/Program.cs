using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace AfinitiPortalAPI
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(Configuration)
               .CreateLogger();
            try
            {
                Log.Information("starting web host");
                Createhostbuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder Createhostbuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webbuilder =>
            {
                webbuilder.UseStartup<Startup>();
            });
    }
}