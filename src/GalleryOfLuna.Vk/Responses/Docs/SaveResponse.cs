namespace GalleryOfLuna.Vk.Responses.Docs
{
    public record SaveResponse(string Type, Document Doc) : IVkResponse;

    // TODO: Is not a full definition of this response
    public record Document(long Id, long OwnerId, string Title, long Size, string Ext, string Url);
}