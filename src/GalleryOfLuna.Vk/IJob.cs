using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk
{
    internal interface IJob
    {
        Task Execute() => Execute(default);

        Task Execute(CancellationToken cancellationToken);
    }
}
