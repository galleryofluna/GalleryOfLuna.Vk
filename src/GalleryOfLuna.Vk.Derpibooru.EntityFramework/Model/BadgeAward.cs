using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class BadgeAward
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BadgeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Label { get; set; }

        public virtual Badge Badge { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
