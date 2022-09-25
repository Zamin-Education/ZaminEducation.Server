using ZaminEducation.Service.DTOs.CoursesDtos;
using ZaminEducation.Service.DTOs.UsersDtos;

namespace ZaminEducation.Service.DTOs.UserCoursesDtos
{
    public class SavedCourseForCreationDto
    {
        public long CourseId { get; set; }

        public long UserId { get; set; }
    }
}
