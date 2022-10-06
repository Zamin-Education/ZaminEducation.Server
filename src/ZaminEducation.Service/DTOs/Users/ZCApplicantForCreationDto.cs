using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZaminEducation.Service.DTOs.Users
{
    public class ZCApplicantForCreationDto
    {
        [MinLength(3),Required,NotNull]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; } 
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [Required,Phone,MinLength(9),MaxLength(23)]
        public string Phone { get; set; }
        public string? ParentPhone { get; set; }
        public bool? AccesToUseMedia { get; set; }
    }
}
