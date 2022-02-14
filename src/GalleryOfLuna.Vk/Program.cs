using FluentValidation;

using GalleryOfLuna.Vk.Configuration;
using GalleryOfLuna.Vk.Derpibooru.EntityFramework;
using GalleryOfLuna.Vk.EntityFramework.Sqlite;
using GalleryOfLuna.Vk.Extensions;
using GalleryOfLuna.Vk.Publishing.EntityFramework;

using IL.FluentValidation.Extensions.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;
using System.IO;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk
{
    public static class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddCommandLine(Environment.GetCommandLineArgs())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .AddEnvironmentVariables()
            .Build();

        public static async Task<int> Main(string[] args)
        {
            // Configuration can be specified by appsettings.json (https://github.com/serilog/serilog-settings-configuration)
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices(ConfigureServices)
                .Build();

            try
            {
                await host.MigrateDatabaseAsync<PublishingDbContext>();
                await host.RunAsync();
                    
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IValidator<TargetsConfiguration>, TargetsConfiguration.Validator>();

            services.AddOptions<DatabaseConfiguration>(Sections.Database)
                .BindConfiguration(Sections.Database)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<DatabaseConfiguration>(Sections.Derpibooru)
                .BindConfiguration(Sections.Derpibooru)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<TargetsConfiguration>(Sections.Targets)
                .BindConfiguration(Sections.Targets)
                .ValidateWithFluentValidator()
                .ValidateOnStart();

            var dbConfiguration = Configuration.GetSection(Sections.Database).Get<DatabaseConfiguration>();
            services.AddDbContextPool<PublishingDbContext, SqlitePublishingDbContext>(
                dbConfiguration.ConnectionString,
                dbConfiguration.Type == DatabaseTypes.Default ? DatabaseTypes.SQLite : dbConfiguration.Type,
                dbConfiguration.MaximumConnections);
            
            dbConfiguration = Configuration.GetSection(Sections.Derpibooru).Get<DatabaseConfiguration>();
            services.AddDbContextPool<DerpibooruDbContext, DerpibooruDbContext>(
                dbConfiguration.ConnectionString,
                DatabaseTypes.PostgreSQL,
                dbConfiguration.MaximumConnections);

            services.AddHealthChecks()
                .AddDbContextCheck<DerpibooruDbContext>()
                .AddDbContextCheck<PublishingDbContext>();

            services.AddHostedService<Worker>();
        }
    }
}