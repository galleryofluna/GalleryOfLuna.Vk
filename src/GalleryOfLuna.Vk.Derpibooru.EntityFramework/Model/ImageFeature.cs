using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ImageFeature
    {
        public long ImageId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Image Image { get; set; } = null!;
    }
}
