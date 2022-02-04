using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Configuration
{
    public enum DatabaseTypes
    {
        Default,

        SQLite,
        PostgreSQL,

#if DEBUG
        InMemory
#endif
    }
}
