using System.ComponentModel.DataAnnotations;

namespace ZaminEducation.Service.DTOs.UsersDtos
{
    public class RegionForCreationDto
    {
        [MaxLength(50), Required]
        public string Name { get; set; }

        public long Parent { get; set; }
    }
}
