using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Responses
{
    public record VkError(Error Error) : IVkResponse;

    public record Error(int ErrorCode, string ErrorMsg, IEnumerable<RequestParameter> RequestParams);

    public record RequestParameter(string Key, string Value);
}
