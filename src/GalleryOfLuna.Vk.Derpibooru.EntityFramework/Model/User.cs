using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class User
    {
        public User()
        {
            ArtistLinks = new HashSet<ArtistLink>();
            BadgeAwards = new HashSet<BadgeAward>();
            Comments = new HashSet<Comment>();
            Galleries = new HashSet<Gallery>();
            Images = new HashSet<Image>();
            Posts = new HashSet<Post>();
            SourceChanges = new HashSet<SourceChange>();
            TagChanges = new HashSet<TagChange>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ArtistLink> ArtistLinks { get; set; }
        public virtual ICollection<BadgeAward> BadgeAwards { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<SourceChange> SourceChanges { get; set; }
        public virtual ICollection<TagChange> TagChanges { get; set; }
    }
}
