namespace GalleryOfLuna.Vk.Publishing.EntityFramework.Model
{
    public class PublishedImage
    {
        public PublishedImage(string provider, string imageId) 
        {
            Provider = provider;
            ImageId = imageId;
        }

        public PublishedImage(DateTime? publishedOn, string provider, string imageId) 
            : this(provider, imageId)
        {
            PublishedOn = publishedOn;
            Skipped = !publishedOn.HasValue;
        }

        public Guid Id { get; set; }
        public DateTime? PublishedOn { get; set; }

        public string Provider { get; set; }
        public string ImageId { get; set; }

        public bool Skipped { get; set; }
    }
}
