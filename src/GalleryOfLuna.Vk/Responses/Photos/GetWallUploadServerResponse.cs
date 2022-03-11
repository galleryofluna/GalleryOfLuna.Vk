using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Responses.Photos
{
    public record GetWallUploadServerResponse(int AlbumId, string UploadUrl, int UserId) : IVkResponse;
}
