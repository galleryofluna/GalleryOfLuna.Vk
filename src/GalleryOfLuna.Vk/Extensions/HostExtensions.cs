using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GalleryOfLuna.Vk.Extensions
{
    public static class HostExtensions
    {
        public static async Task<IHost> MigrateDatabaseAsync<TDbContext>(this IHost host)
            where TDbContext : DbContext
        {
            await using var scope = host.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>() as DbContext;

            if (context == null)
                throw new ArgumentException(
                    $"Can't migrate {typeof(TDbContext).Name} service, because it's not a Entity Framework database context.");

            if (context.Database.IsInMemory())
                return host;

            await context.Database.MigrateAsync();

            return host;
        }
    }
}
