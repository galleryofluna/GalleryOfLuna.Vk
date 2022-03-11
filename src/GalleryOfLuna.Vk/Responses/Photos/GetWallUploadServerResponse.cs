namespace GalleryOfLuna.Vk.Responses.Photos
{
    public record GetWallUploadServerResponse(int AlbumId, string UploadUrl, int UserId) : IVkResponse;
}