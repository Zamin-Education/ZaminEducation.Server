using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Entities.Commons;

namespace ZaminEducation.Domain.Entities.Quizzes
{
    public sealed class QuizAsset : Auditable
    {
        public long QuizContentId { get; set; }
        public QuizContent Content { get; set; }

        public long FileId { get; set; }
        public Attachment File { get; set; }
    }
}