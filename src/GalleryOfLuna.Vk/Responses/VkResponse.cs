using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Responses
{
    public record VkResponse<T>(T Response)
    {
        public static implicit operator T(VkResponse<T> response) => response.Response;
    }
}
