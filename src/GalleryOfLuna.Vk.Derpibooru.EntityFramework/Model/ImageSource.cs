using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ImageSource
    {
        public long ImageId { get; set; }
        public string Source { get; set; } = null!;

        public virtual Image Image { get; set; } = null!;
    }
}
