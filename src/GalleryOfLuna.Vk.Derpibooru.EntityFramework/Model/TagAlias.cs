using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class TagAlias
    {
        public long TagId { get; set; }
        public long TargetTagId { get; set; }

        public virtual Tag Tag { get; set; } = null!;
        public virtual Tag TargetTag { get; set; } = null!;
    }
}
