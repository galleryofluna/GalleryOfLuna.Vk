using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class Gallery
    {
        public long Id { get; set; }
        public long ThumbnailId { get; set; }
        public long UserId { get; set; }
        public long ImageCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Title { get; set; } = null!;
        public string SpoilerWarning { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual Image Thumbnail { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
