namespace ZaminEducation.Service.DTOs.QuizzesDtos
{
    public class QuestionAnswerForCreationDto
    {
        public long ContentId { get; set; }

        public bool IsCorrect { get; set; }

        public long QuizId { get; set; }

        public long Question { get; set; }
    }
}
