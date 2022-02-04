using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Configuration
{
    public class DatabaseConfiguration
    {
        public DatabaseTypes Type { get; set; } = DatabaseTypes.Default;

        [Required] 
        public string ConnectionString { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int MaximumConnections { get; set; } = 1024;
    }
}
