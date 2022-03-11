using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Responses.Photos
{
    // TODO: Is not a full definition of this response
    public record PhotoResponse(long Id, long OwnerId): IVkResponse;
}
