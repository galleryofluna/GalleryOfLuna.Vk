using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class Forum
    {
        public Forum()
        {
            Topics = new HashSet<Topic>();
        }

        public long Id { get; set; }
        public long TopicCount { get; set; }
        public long PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Topic> Topics { get; set; }
    }
}
