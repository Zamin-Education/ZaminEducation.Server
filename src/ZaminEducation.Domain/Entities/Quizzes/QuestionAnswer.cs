using System.ComponentModel.DataAnnotations.Schema;
using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.Quizzes
{
    public sealed class QuestionAnswer : Auditable
    {
        public long ContentId { get; set; }

        [ForeignKey(nameof(ContentId))]
        public QuizContent Content { get; set; }

        public bool IsCorrect { get; set; }

        public long QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public long QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public QuizContent Question { get; set; }
    }
}