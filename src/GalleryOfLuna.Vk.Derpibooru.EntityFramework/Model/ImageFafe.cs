using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ImageFafe
    {
        public DateTime CreatedAt { get; set; }
        public long ImageId { get; set; }
        public long UserId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
