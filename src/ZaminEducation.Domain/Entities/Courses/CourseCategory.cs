using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.Courses
{
    public sealed class CourseCategory : Auditable
    {
        public string Name { get; set; }
    }
}