using ZaminEducation.Service.DTOs.CoursesDtos;

namespace ZaminEducation.Service.DTOs.QuizzesDtos
{
    public class QuizForCreationDto
    {
        public long CourseId { get; set; }

        public long QuestionId { get; set; }
    }
}
