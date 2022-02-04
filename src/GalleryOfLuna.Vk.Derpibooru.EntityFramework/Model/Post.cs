using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class Post
    {
        public long Id { get; set; }
        public long TopicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public string Body { get; set; } = null!;

        public virtual User? User { get; set; }
    }
}
