using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class GalleryInteraction
    {
        public long ImageId { get; set; }
        public long GalleryId { get; set; }
        public int Position { get; set; }
    }
}
