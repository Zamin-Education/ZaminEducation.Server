using System.ComponentModel.DataAnnotations.Schema;
using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.Courses
{
    public class CourseModule : Auditable
    {
        public string Name { get; set; }

        public long CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }

        public virtual ICollection<CourseVideo> Videos { get; set; }
    }
}