using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Configuration
{
    public class VkConfiguration
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public long GroupId { get; set; }
    }
}
