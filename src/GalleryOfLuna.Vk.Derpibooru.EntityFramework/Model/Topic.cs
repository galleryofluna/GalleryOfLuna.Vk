using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class Topic
    {
        public long Id { get; set; }
        public long ForumId { get; set; }
        public long PostCount { get; set; }
        public long ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Sticky { get; set; }
        public bool Locked { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public virtual Forum Forum { get; set; } = null!;
    }
}
