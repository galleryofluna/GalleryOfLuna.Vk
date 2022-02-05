using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model;

namespace GalleryOfLuna.Vk.Derpibooru.EntityFramework
{
    public partial class DerpibooruDbContext : DbContext
    {
        public DerpibooruDbContext(DbContextOptions<DerpibooruDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArtistLink> ArtistLinks { get; set; } = null!;
        public virtual DbSet<Badge> Badges { get; set; } = null!;
        public virtual DbSet<BadgeAward> BadgeAwards { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<DuplicateReport> DuplicateReports { get; set; } = null!;
        public virtual DbSet<Forum> Forums { get; set; } = null!;
        public virtual DbSet<Gallery> Galleries { get; set; } = null!;
        public virtual DbSet<GalleryInteraction> GalleryInteractions { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<ImageDuplicate> ImageDuplicates { get; set; } = null!;
        public virtual DbSet<ImageFafe> ImageFaves { get; set; } = null!;
        public virtual DbSet<ImageFeature> ImageFeatures { get; set; } = null!;
        public virtual DbSet<ImageHide> ImageHides { get; set; } = null!;
        public virtual DbSet<ImageIntensity> ImageIntensities { get; set; } = null!;
        public virtual DbSet<ImageSource> ImageSources { get; set; } = null!;
        public virtual DbSet<ImageTagging> ImageTaggings { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<SourceChange> SourceChanges { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<TagAlias> TagAliases { get; set; } = null!;
        public virtual DbSet<TagChange> TagChanges { get; set; } = null!;
        public virtual DbSet<TagImplication> TagImplications { get; set; } = null!;
        public virtual DbSet<Topic> Topics { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtistLink>(entity =>
            {
                entity.ToTable("artist_links");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ArtistLinks)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("fk_artist_links_tags");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ArtistLinks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_artist_links_users");
            });

            modelBuilder.Entity<Badge>(entity =>
            {
                entity.ToTable("badges");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageUrl).HasColumnName("image_url");

                entity.Property(e => e.Title).HasColumnName("title");
            });

            modelBuilder.Entity<BadgeAward>(entity =>
            {
                entity.ToTable("badge_awards");

                entity.HasIndex(e => e.BadgeId, "index_badge_awards_on_badge_id");

                entity.HasIndex(e => new { e.UserId, e.BadgeId }, "index_badge_awards_on_user_id_and_badge_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BadgeId).HasColumnName("badge_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Label).HasColumnName("label");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Badge)
                    .WithMany(p => p.BadgeAwards)
                    .HasForeignKey(d => d.BadgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_badge_awards_badges");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BadgeAwards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_badge_awards_users");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comments");

                entity.HasIndex(e => new { e.ImageId, e.CreatedAt }, "index_comments_on_image_id_and_created_at");

                entity.HasIndex(e => e.UserId, "index_comments_on_user_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Body).HasColumnName("body");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_comments_images");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_comments_users");
            });

            modelBuilder.Entity<DuplicateReport>(entity =>
            {
                entity.ToTable("duplicate_reports");

                entity.HasIndex(e => e.DuplicateOfImageId, "index_duplicate_reports_on_duplicate_of_image_id");

                entity.HasIndex(e => e.ImageId, "index_duplicate_reports_on_image_id");

                entity.HasIndex(e => e.UserId, "index_duplicate_reports_on_user_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.DuplicateOfImageId).HasColumnName("duplicate_of_image_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.DuplicateOfImage)
                    .WithMany(p => p.DuplicateReportDuplicateOfImages)
                    .HasForeignKey(d => d.DuplicateOfImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_duplicate_reports_image_target");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.DuplicateReportImages)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_duplicate_reports_image_source");
            });

            modelBuilder.Entity<Forum>(entity =>
            {
                entity.ToTable("forums");

                entity.HasIndex(e => e.ShortName, "index_forums_on_short_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.PostCount).HasColumnName("post_count");

                entity.Property(e => e.ShortName).HasColumnName("short_name");

                entity.Property(e => e.TopicCount).HasColumnName("topic_count");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.ToTable("galleries");

                entity.HasIndex(e => e.ThumbnailId, "index_galleries_on_thumbnail_id");

                entity.HasIndex(e => e.UserId, "index_galleries_on_user_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageCount).HasColumnName("image_count");

                entity.Property(e => e.SpoilerWarning).HasColumnName("spoiler_warning");

                entity.Property(e => e.ThumbnailId).HasColumnName("thumbnail_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Thumbnail)
                    .WithMany(p => p.Galleries)
                    .HasForeignKey(d => d.ThumbnailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_galleries_images");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Galleries)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_galleries_users");
            });

            modelBuilder.Entity<GalleryInteraction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gallery_interactions");

                entity.HasIndex(e => e.GalleryId, "index_gallery_interactions_on_gallery_id");

                entity.HasIndex(e => new { e.ImageId, e.GalleryId }, "index_gallery_interactions_on_image_id_and_gallery_id")
                    .IsUnique();

                entity.Property(e => e.GalleryId).HasColumnName("gallery_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.Position).HasColumnName("position");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("images");

                entity.HasIndex(e => e.WilsonScore, "images_ix_wilson_score");

                entity.HasIndex(e => e.UserId, "index_images_on_user_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CommentCount).HasColumnName("comment_count");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Downvotes).HasColumnName("downvotes");

                entity.Property(e => e.Favorites).HasColumnName("favorites");

                entity.Property(e => e.HiddenFromUsers).HasColumnName("hidden_from_users");

                entity.Property(e => e.Hides).HasColumnName("hides");

                entity.Property(e => e.ImageAspectRatio).HasColumnName("image_aspect_ratio");

                entity.Property(e => e.ImageFormat).HasColumnName("image_format");

                entity.Property(e => e.ImageHeight).HasColumnName("image_height");

                entity.Property(e => e.ImageMimeType).HasColumnName("image_mime_type");

                entity.Property(e => e.ImageName).HasColumnName("image_name");

                entity.Property(e => e.ImageOrigSha512Hash).HasColumnName("image_orig_sha512_hash");

                entity.Property(e => e.ImageSha512Hash).HasColumnName("image_sha512_hash");

                entity.Property(e => e.ImageSize).HasColumnName("image_size");

                entity.Property(e => e.ImageWidth).HasColumnName("image_width");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Upvotes).HasColumnName("upvotes");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VersionPath).HasColumnName("version_path");

                entity.Property(e => e.WilsonScore).HasColumnName("wilson_score");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_images_users");
            });

            modelBuilder.Entity<ImageDuplicate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("image_duplicates");

                entity.HasIndex(e => new { e.ImageId, e.TargetId }, "index_image_duplicates_on_image_id_and_target_id")
                    .IsUnique();

                entity.HasIndex(e => e.TargetId, "index_image_duplicates_on_target_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.TargetId).HasColumnName("target_id");

                entity.HasOne(d => d.Image)
                    .WithMany()
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_duplicates_image_source");

                entity.HasOne(d => d.Target)
                    .WithMany()
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_duplicates_image_target");
            });

            modelBuilder.Entity<ImageFafe>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("image_faves");

                entity.HasIndex(e => new { e.ImageId, e.UserId }, "index_image_faves_on_image_id_and_user_id")
                    .IsUnique();

                entity.HasIndex(e => e.UserId, "index_image_faves_on_user_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Image)
                    .WithMany()
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_faves_images");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_faves_users");
            });

            modelBuilder.Entity<ImageFeature>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("image_features");

                entity.HasIndex(e => e.ImageId, "index_image_features_on_image_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.HasOne(d => d.Image)
                    .WithMany()
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_features_images");
            });

            modelBuilder.Entity<ImageHide>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("image_hides");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.Reason).HasColumnName("reason");
            });

            modelBuilder.Entity<ImageIntensity>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("image_intensities");

                entity.HasIndex(e => e.ImageId, "index_image_intensities_on_image_id")
                    .IsUnique();

                entity.HasIndex(e => new { e.NwIntensity, e.NeIntensity, e.SwIntensity, e.SeIntensity }, "intensities_index");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.NeIntensity).HasColumnName("ne_intensity");

                entity.Property(e => e.NwIntensity).HasColumnName("nw_intensity");

                entity.Property(e => e.SeIntensity).HasColumnName("se_intensity");

                entity.Property(e => e.SwIntensity).HasColumnName("sw_intensity");

                entity.HasOne(d => d.Image)
                    .WithOne()
                    .HasForeignKey<ImageIntensity>(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_intensities_images");
            });

            modelBuilder.Entity<ImageSource>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("image_sources");

                entity.HasIndex(e => e.ImageId, "index_image_sources_on_image_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.Source).HasColumnName("source");

                entity.HasOne(d => d.Image)
                    .WithMany()
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_sources_images");
            });

            modelBuilder.Entity<ImageTagging>(entity =>
            {
                entity.HasKey(d => new { d.ImageId, d.TagId });

                entity.ToTable("image_taggings");

                entity.HasIndex(e => new { e.ImageId, e.TagId }, "index_image_taggings_on_image_id_and_tag_id")
                    .IsUnique();

                entity.HasIndex(e => e.TagId, "index_image_taggings_on_tag_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.HasOne(d => d.Image)
                    .WithMany(d => d.ImageTaggings)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_taggings_images");

                entity.HasOne(d => d.Tag)
                    .WithMany(d=> d.ImageTaggings)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_image_taggings_tags");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.HasIndex(e => e.UserId, "index_posts_on_user_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Body).HasColumnName("body");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.TopicId).HasColumnName("topic_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_posts_users");
            });

            modelBuilder.Entity<SourceChange>(entity =>
            {
                entity.ToTable("source_changes");

                entity.HasIndex(e => e.ImageId, "index_source_changes_on_image_id");

                entity.HasIndex(e => e.UserId, "index_source_changes_on_user_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.NewValue).HasColumnName("new_value");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.SourceChanges)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_source_changes_images");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SourceChanges)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_source_changes_users");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tags");

                entity.HasIndex(e => e.Name, "index_tags_on_name")
                    .IsUnique();

                entity.HasIndex(e => e.Slug, "index_tags_on_slug")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageCount).HasColumnName("image_count");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.ShortDescription).HasColumnName("short_description");

                entity.Property(e => e.Slug).HasColumnName("slug");
            });

            modelBuilder.Entity<TagAlias>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tag_aliases");

                entity.HasIndex(e => e.TagId, "index_tag_aliases_on_tag_id")
                    .IsUnique();

                entity.HasIndex(e => e.TargetTagId, "index_tag_aliases_on_target_tag_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.TargetTagId).HasColumnName("target_tag_id");

                entity.HasOne(d => d.Tag)
                    .WithOne()
                    .HasForeignKey<TagAlias>(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tag_changes_tag_source");

                entity.HasOne(d => d.TargetTag)
                    .WithMany()
                    .HasForeignKey(d => d.TargetTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tag_changes_tag_target");
            });

            modelBuilder.Entity<TagChange>(entity =>
            {
                entity.ToTable("tag_changes");

                entity.HasIndex(e => e.ImageId, "index_tag_changes_on_image_id");

                entity.HasIndex(e => e.TagId, "index_tag_changes_on_tag_id");

                entity.HasIndex(e => e.UserId, "index_tag_changes_on_user_id");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Added).HasColumnName("added");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.TagNameCache).HasColumnName("tag_name_cache");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.TagChanges)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tag_changes_images");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagChanges)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("fk_tag_changes_tags");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TagChanges)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_tag_changes_users");
            });

            modelBuilder.Entity<TagImplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tag_implications");

                entity.HasIndex(e => new { e.TagId, e.TargetTagId }, "index_tag_implications_on_tag_id_and_target_tag_id")
                    .IsUnique();

                entity.HasIndex(e => e.TargetTagId, "index_tag_implications_on_target_tag_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.TargetTagId).HasColumnName("target_tag_id");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tag_changes_tag_source");

                entity.HasOne(d => d.TargetTag)
                    .WithMany()
                    .HasForeignKey(d => d.TargetTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tag_changes_tag_target");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("topics");

                entity.HasIndex(e => e.ForumId, "index_topics_on_forum_id");

                entity.HasIndex(e => e.Slug, "index_topics_on_slug");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.ForumId).HasColumnName("forum_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.PostCount).HasColumnName("post_count");

                entity.Property(e => e.Slug).HasColumnName("slug");

                entity.Property(e => e.Sticky).HasColumnName("sticky");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated_at");

                entity.Property(e => e.ViewCount).HasColumnName("view_count");

                entity.HasOne(d => d.Forum)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.ForumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_topics_forums");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Name, "index_users_on_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
