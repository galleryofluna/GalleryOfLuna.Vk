using GalleryOfLuna.Vk.Publishing.EntityFramework.Model;
using Microsoft.EntityFrameworkCore;

namespace GalleryOfLuna.Vk.Publishing.EntityFramework
{
    public class PublishingDbContext : DbContext
    {
        public DbSet<PublishedImage> PublishedImages { get; set; } = null!;

        public PublishingDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
