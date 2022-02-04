using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class DuplicateReport
    {
        public long Id { get; set; }
        public long ImageId { get; set; }
        public long DuplicateOfImageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public string State { get; set; } = null!;
        public string? Reason { get; set; }

        public virtual Image DuplicateOfImage { get; set; } = null!;
        public virtual Image Image { get; set; } = null!;
    }
}
