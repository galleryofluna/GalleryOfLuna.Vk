using Microsoft.EntityFrameworkCore;

namespace GalleryOfLuna.Vk.Publishing.EntityFramework.PostgreSQL
{
    public sealed class PostgreSqlPublishingDbContext : PublishingDbContext
    {
        public PostgreSqlPublishingDbContext(DbContextOptions<PostgreSqlPublishingDbContext> options) : base(options)
        {
        }
    }
}