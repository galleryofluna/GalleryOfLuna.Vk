namespace GalleryOfLuna.Vk.Responses
{
    public record VkError(Error Error) : IVkResponse;

    public record Error(int ErrorCode, string ErrorMsg, IEnumerable<RequestParameter> RequestParams);

    public record RequestParameter(string Key, string Value);
}