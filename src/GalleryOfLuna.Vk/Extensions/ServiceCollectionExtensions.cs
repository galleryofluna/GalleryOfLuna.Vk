using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GalleryOfLuna.Vk.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GalleryOfLuna.Vk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContextPool<TContextService, TContextImplementation>(
            this IServiceCollection services,
            string connectionString, DatabaseTypes databaseType, int poolSize)
            where TContextService : class
            where TContextImplementation : DbContext, TContextService
        {
            var migrationAssembly = typeof(TContextImplementation).GetTypeInfo().Assembly.GetName().Name;
            switch (databaseType)
            {
                case DatabaseTypes.SQLite:
                    services.AddDbContextPool<TContextService, TContextImplementation>(options =>
                    {
                        options.UseSqlite(connectionString, sqliteOptions => sqliteOptions.MigrationsAssembly(migrationAssembly));
                    }, poolSize);

                    break;

#if DEBUG
                case DatabaseTypes.InMemory:
                    services.AddDbContextPool<TContextService, TContextImplementation>(options =>
                    {
                        options.UseInMemoryDatabase(typeof(TContextService).Name);
                    }, poolSize);
                    break;
#endif

                
                case DatabaseTypes.PostgreSQL:
                    services.AddDbContextPool<TContextService, TContextImplementation>(options =>
                    {
                        
                        options.UseNpgsql(connectionString, npsqlOptions => npsqlOptions.MigrationsAssembly(migrationAssembly));
                    }, poolSize);
                    break;

                case DatabaseTypes.Default:
                default:
                    throw new ArgumentException("Default RDBMS registration is not allowed. Please specify RDBMS type explicitly", nameof(databaseType));
            }

            return services;
        }
    }
}
