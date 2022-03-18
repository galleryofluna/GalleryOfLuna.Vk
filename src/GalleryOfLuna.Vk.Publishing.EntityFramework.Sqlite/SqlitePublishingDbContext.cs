using GalleryOfLuna.Vk.Publishing.EntityFramework;

using Microsoft.EntityFrameworkCore;

namespace GalleryOfLuna.Vk.EntityFramework.Sqlite
{
    public sealed class SqlitePublishingDbContext : PublishingDbContext
    {
        public SqlitePublishingDbContext(DbContextOptions<SqlitePublishingDbContext> options) : base(options)
        {
        }
    }
}