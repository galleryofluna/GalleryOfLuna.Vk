using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class SourceChange
    {
        public long Id { get; set; }
        public long ImageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public string NewValue { get; set; } = null!;

        public virtual Image Image { get; set; } = null!;
        public virtual User? User { get; set; }
    }
}
