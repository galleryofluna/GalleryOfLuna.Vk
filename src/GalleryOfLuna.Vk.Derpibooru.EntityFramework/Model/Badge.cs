using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class Badge
    {
        public Badge()
        {
            BadgeAwards = new HashSet<BadgeAward>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

        public virtual ICollection<BadgeAward> BadgeAwards { get; set; }
    }
}
