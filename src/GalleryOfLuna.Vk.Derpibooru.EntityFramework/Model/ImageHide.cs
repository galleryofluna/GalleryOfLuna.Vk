using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ImageHide
    {
        public long ImageId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
