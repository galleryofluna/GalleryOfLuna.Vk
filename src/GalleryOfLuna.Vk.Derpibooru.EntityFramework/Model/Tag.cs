using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class Tag
    {
        public Tag()
        {
            ArtistLinks = new HashSet<ArtistLink>();
            TagChanges = new HashSet<TagChange>();
        }

        public long Id { get; set; }
        public long ImageCount { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }

        public virtual ICollection<ArtistLink> ArtistLinks { get; set; }
        public virtual ICollection<TagChange> TagChanges { get; set; }
        public virtual ICollection<ImageTagging> ImageTaggings { get; set; }
    }
}
