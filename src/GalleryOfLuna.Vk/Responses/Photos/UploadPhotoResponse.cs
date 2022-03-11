namespace GalleryOfLuna.Vk.Responses.Photos
{
    public record UploadPhotoResponse(long Server, string Photo, string Hash) : IVkResponse;
}