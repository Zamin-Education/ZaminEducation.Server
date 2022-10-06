using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Domain.Enums;

namespace ZaminEducation.Domain.Entities.Users
{
    public class ZCApplicant : Auditable
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserRole UserRole { get; set; }

        public long DirectoryId { get; set; }
        public ZCApplicantDirection Directory { get; set; }

        public string Phone { get; set; }
        public string? ParentPhone { get; set; }
        public bool AccesToUseMedia { get; set; } = false;
    }
}
