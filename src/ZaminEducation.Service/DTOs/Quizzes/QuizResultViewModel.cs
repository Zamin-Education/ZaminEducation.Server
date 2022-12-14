using ZaminEducation.Domain.Entities.Quizzes;

namespace ZaminEducation.Service.ViewModels.Quizzes;

public class QuizResultViewModel
{
    public Quiz Quiz { get; set; }
    public QuestionAnswer Choice { get; set; }

    public bool IsCorrect { get; set; }
}