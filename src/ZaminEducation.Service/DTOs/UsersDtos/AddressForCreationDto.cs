using System.ComponentModel.DataAnnotations;

namespace ZaminEducation.Service.DTOs.UsersDtos
{
    public class AddressForCreationDto
    {
        public long CountryId { get; set; }

        public long RegionId { get; set; }

        [MaxLength(50), Required]
        public string District { get; set; }

        [MaxLength(100), Required]
        public string AddressLine { get; set; }
    }
}
