using AfinitiPortalAPI.Shared.Crypto;
using AfinitiPortalAPI.Shared.Library.TrackerApi.Attributes;
using AfinitiPortalAPI.Shared.Library.TrackerApi.Model;
using AfinitiPortalAPI.Shared.Shared.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Tracker.API
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        public static AppSettings AppSettings { get; set; } = new AppSettings();

        public static void Main(string[] args)
        {
            // Bind AppSettings.json to strongly typed object...
            Configuration.GetSection("AppSettings").Bind(AppSettings);

            // Init Logging...
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console(
                    LogEventLevel.Debug)
                .WriteTo.File(
                    path: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log\\EP\\.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.MSSqlServer(
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    connectionString: CryptoUtils.AES.Decrypt(AppSettings.DBConnectionString),
                    sinkOptions: new MSSqlServerSinkOptions()
                    {
                        SchemaName = AppSettings.TrackerApi.Schema,
                        TableName = AppSettings.TrackerApi.Table,
                        BatchPostingLimit = AppSettings.TrackerApi.BatchPostingLimit,
                        BatchPeriod = TimeSpan.FromSeconds(AppSettings.TrackerApi.BatchPeriodInSeconds),
                        AutoCreateSqlTable = false
                    },
                    columnOptions: GetColumnOptions())
                .CreateLogger();

            // Self logging of Serilog...
            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Console.WriteLine(msg);
                Log.Fatal(msg);
                Debugger.Break();
            });

            try
            {
                Log.Debug("Starting Host...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            // Make sure to flush Serilog on process kill...
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(Configuration);
                    webBuilder.UseSerilog();
                    webBuilder.UseStartup<Startup>();
                });

        public static ColumnOptions GetColumnOptions()
        {
            var options = new ColumnOptions();

            // Remove all standard columns...
            options.Store.Remove(StandardColumn.MessageTemplate);
            options.Store.Remove(StandardColumn.Exception);
            options.Store.Remove(StandardColumn.LogEvent);
            options.Store.Remove(StandardColumn.Level);
            options.Store.Remove(StandardColumn.Message);
            options.Store.Remove(StandardColumn.Properties);
            options.Store.Remove(StandardColumn.TimeStamp);
            options.Store.Remove(StandardColumn.Id);

            // Add columns...
            var dbColumns = typeof(AuditModel)
                .GetProperties()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(SqlDbTypeAttribute)))
                .Select(x =>
                {
                    var reflectedProp = ((SqlDbTypeAttribute)x.GetCustomAttributes(typeof(SqlDbTypeAttribute), false).Single());
                    var result = new SqlColumn
                    {
                        ColumnName = x.Name,
                        DataType = reflectedProp.DBType,
                        AllowNull = reflectedProp.Nullable
                    };

                    if (reflectedProp.DataLength.HasValue)
                        result.DataLength = reflectedProp.DataLength.Value;

                    return result;
                }).ToList();

            options.AdditionalColumns = dbColumns;

            // Set PK column...
            options.PrimaryKey.ColumnName = "Id";

            return options;
        }
    }
}
