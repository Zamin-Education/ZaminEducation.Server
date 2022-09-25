
using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.Users;

namespace ZaminEducation.Domain.Entities.UserCourses
{
    public sealed class SavedCourse : Auditable
    {
        public long CourseId { get; set; }
        public Course Course { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}