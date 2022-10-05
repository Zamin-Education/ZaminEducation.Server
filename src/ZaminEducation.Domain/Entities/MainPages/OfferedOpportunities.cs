using Newtonsoft.Json;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.MainPages.Commons;

namespace ZaminEducation.Domain.Entities.MainPages;

public class OfferedOpportunities : Text
{
    public ICollection<Reason> Opportunities { get; set; }

    [JsonIgnore]
    public long AttachmentId { get; set; }
    public Attachment Attachment { get; set; }

    [JsonIgnore]
    public HomePage HomePage { get; set; }
}