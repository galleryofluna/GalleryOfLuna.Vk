namespace GalleryOfLuna.Vk.Publishing.EntityFramework.Model
{
    public class PublishedImage
    {
        public PublishedImage(DateTime publishedOn, string provider, string imageId)
        {
            Provider = provider;
            ImageId = imageId;
            PublishedOn = publishedOn;
        }

        public Guid Id { get; set; }
        public DateTime PublishedOn { get; set; }

        public string Provider { get; set; }
        public string ImageId { get; set; }
    }
}
