using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.Users
{
    public sealed class Region : Auditable
    {
        public string Name { get; set; }

        public long? ParentId { get; set; }
        public Region Parent { get; set; }
    }
}