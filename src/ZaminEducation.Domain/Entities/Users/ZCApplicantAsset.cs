using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Entities.Commons;

namespace ZaminEducation.Domain.Entities.Users
{
    public class ZCApplicantAsset : Auditable
    {
        public long UserId { get; set; }
        public ZCApplicant User { get; set; }

        public long FileId { get; set; }
        public Attachment File { get; set; }
    }
}
