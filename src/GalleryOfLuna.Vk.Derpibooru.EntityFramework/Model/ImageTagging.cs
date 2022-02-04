using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ImageTagging
    {
        public long ImageId { get; set; }
        public long TagId { get; set; }

        public virtual Image Image { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
