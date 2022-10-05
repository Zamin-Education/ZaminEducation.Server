using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.MainPages.Commons;

namespace ZaminEducation.Domain.Entities.MainPages;

public class PopularCourses : Text
{
    public ICollection<Course> Courses { get; set; }
}