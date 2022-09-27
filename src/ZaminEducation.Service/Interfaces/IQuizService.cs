using ZaminEducation.Domain.Entities.Quizzes;
using ZaminEducation.Service.DTOs.Quizzes;

namespace ZaminEducation.Service.Interfaces
{
    public interface IQuizService
    {
        ValueTask<Quiz> CreateAsync(QuizForCreationDto quizForCreationDto);
    }
}
