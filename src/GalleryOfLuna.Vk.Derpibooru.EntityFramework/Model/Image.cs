using System;
using System.Collections.Generic;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model
{
    public partial class Image
    {
        public Image()
        {
            Comments = new HashSet<Comment>();
            DuplicateReportDuplicateOfImages = new HashSet<DuplicateReport>();
            DuplicateReportImages = new HashSet<DuplicateReport>();
            Galleries = new HashSet<Gallery>();
            SourceChanges = new HashSet<SourceChange>();
            TagChanges = new HashSet<TagChange>();
        }

        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int ImageSize { get; set; }
        public int CommentCount { get; set; }
        public int Score { get; set; }
        public int Favorites { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int Hides { get; set; }
        public double ImageAspectRatio { get; set; }
        public long? UserId { get; set; }
        public bool HiddenFromUsers { get; set; }
        public string ImageMimeType { get; set; } = null!;
        public string ImageFormat { get; set; } = null!;
        public string ImageName { get; set; } = null!;
        public string VersionPath { get; set; } = null!;
        public string? ImageSha512Hash { get; set; }
        public string? ImageOrigSha512Hash { get; set; }
        public decimal? WilsonScore { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<DuplicateReport> DuplicateReportDuplicateOfImages { get; set; }
        public virtual ICollection<DuplicateReport> DuplicateReportImages { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<SourceChange> SourceChanges { get; set; }
        public virtual ICollection<TagChange> TagChanges { get; set; }
    }
}
