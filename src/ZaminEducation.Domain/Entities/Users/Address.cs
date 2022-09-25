using System.ComponentModel.DataAnnotations;
using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.Users
{
    public sealed class Address : Auditable
    {
        public long CountryId { get; set; }
        public Region Country { get; set; }

        public long RegionId { get; set; }
        public Region Region { get; set; }

        [MaxLength(50)]
        public string District { get; set; }

        [MaxLength(100)]
        public string AddressLine { get; set; }
    }
}