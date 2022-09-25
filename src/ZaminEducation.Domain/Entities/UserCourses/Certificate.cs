
using System.ComponentModel.DataAnnotations.Schema;
using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.Users;

namespace ZaminEducation.Domain.Entities.UserCourses
{
    public sealed class Certificate : Auditable
    {
        public long CourseId { get; set; }
        public Course Course { get; set; }

        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}