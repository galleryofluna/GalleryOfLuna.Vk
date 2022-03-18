namespace GalleryOfLuna.Vk.Responses.Video
{
    public record SaveResponse(
        string AccessKey,
        string Description,
        long OwnerId,
        string Title,
        string UploadUrl,
        long VideoId) : IVkResponse;
}