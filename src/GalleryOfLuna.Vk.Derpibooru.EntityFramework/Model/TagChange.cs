using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class TagChange
    {
        public long Id { get; set; }
        public long ImageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public long? TagId { get; set; }
        public bool Added { get; set; }
        public string TagNameCache { get; set; } = null!;

        public virtual Image Image { get; set; } = null!;
        public virtual Tag? Tag { get; set; }
        public virtual User? User { get; set; }
    }
}
