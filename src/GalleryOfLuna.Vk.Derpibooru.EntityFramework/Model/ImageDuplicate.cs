using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ImageDuplicate
    {
        public long ImageId { get; set; }
        public long TargetId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Image Target { get; set; } = null!;
    }
}
