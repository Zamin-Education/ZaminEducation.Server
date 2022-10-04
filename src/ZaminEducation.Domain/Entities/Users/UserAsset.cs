using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Entities.Commons;

namespace ZaminEducation.Domain.Entities.Users
{
    public class UserAsset : Auditable
    {
        public long UserId { get; set; }
        public ApplicantUser User { get; set; }

        public long FileId { get; set; }
        public Attachment File { get; set; }
    }
}
