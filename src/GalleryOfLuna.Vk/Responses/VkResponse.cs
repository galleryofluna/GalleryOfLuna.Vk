namespace GalleryOfLuna.Vk.Responses
{
    public record VkResponse<T>(T Response)
    {
        public static implicit operator T(VkResponse<T> response) => response.Response;
    }
}