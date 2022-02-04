using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ImageIntensity
    {
        public long ImageId { get; set; }
        public double NwIntensity { get; set; }
        public double NeIntensity { get; set; }
        public double SwIntensity { get; set; }
        public double SeIntensity { get; set; }

        public virtual Image Image { get; set; } = null!;
    }
}
