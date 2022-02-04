using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class ArtistLink
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long? TagId { get; set; }
        public string? Url { get; set; }

        public virtual Tag? Tag { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
