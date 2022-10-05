using Newtonsoft.Json;
using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.MainPages;

public class SocialNetworks : Auditable
{
    public string EmailLink { get; set; }
    public string FacebookLink { get; set; }
    public string InstagrammLink { get; set; }
    public string TelegramLink { get; set; }
    public string YouTubeLink { get; set; }

    [JsonIgnore]
    public HomePage HomePage { get; set; }
}