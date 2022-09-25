using ZaminEducation.Service.DTOs.CoursesDtos;
using ZaminEducation.Service.DTOs.UsersDtos;

namespace ZaminEducation.Service.DTOs.UserCoursesDtos
{
    public class CourseCommentForCreationDto
    {
        public string Text { get; set; }

        public long UserId { get; set; }

        public long CourseId { get; set; }

        public long ParentId { get; set; }
    }
}

