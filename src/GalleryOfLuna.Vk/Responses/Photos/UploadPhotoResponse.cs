using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Responses.Photos
{
    public record UploadPhotoResponse(long Server, string Photo, string Hash) : IVkResponse;
}
