using System.ComponentModel.DataAnnotations;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.DTOs.Commons;

namespace ZaminEducation.Service.DTOs.UsersDtos
{
    public class UserForCreationDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MinLength(5)]
        public string Password { get; set; }

        public long Address { get; set; }

        [MaxLength(200)]
        public string Bio { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public long Image { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public DateTime? DateOfBirth { get; set; }
    }
}
