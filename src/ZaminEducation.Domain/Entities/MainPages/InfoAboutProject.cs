using Newtonsoft.Json;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.MainPages.Commons;

namespace ZaminEducation.Domain.Entities.MainPages;

public class InfoAboutProject : Text
{
    [JsonIgnore]
    public long ImageId { get; set; }
    public Attachment Image { get; set; }

    [JsonIgnore]
    public HomePage HomePage { get; set; }
}