using ZaminEducation.Service.DTOs.CoursesDtos;
using ZaminEducation.Service.DTOs.UsersDtos;

namespace ZaminEducation.Service.DTOs.UserCoursesDtos
{
    public class CourseRateForCreationDto
    {
        public long UserId { get; set; }

        public long CourseId { get; set; }

        public byte Value { get; set; }
    }
}
