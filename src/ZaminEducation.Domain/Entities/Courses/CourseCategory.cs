using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.Courses
{
    public class CourseCategory : Auditable
    {
        public string Name { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}