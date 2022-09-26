using ZaminEducation.Service.DTOs.CoursesDtos;
using ZaminEducation.Service.DTOs.UsersDtos;

namespace ZaminEducation.Service.DTOs.QuizzesDtos
{
    public class QuizResultForCreationDto
    {
        public long UserId { get; set; }

        public long CourseId { get; set; }

        public double Percentage { get; set; }

        public long? AttachmentId { get; set; }
    }
}
