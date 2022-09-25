
using System.ComponentModel.DataAnnotations;
using ZaminEducation.Domain.Commons;

namespace ZaminEducation.Domain.Entities.Users
{
    public sealed class UserSocialNetwork : Auditable
    {
        public long UserId { get; set; }
        public User User { get; set; }

        [DataType(DataType.Url)]
        public string TelegramLink { get; set; }

        [DataType(DataType.Url)]
        public string InstagramLink { get; set; }

        [DataType(DataType.Url)]
        public string FacebookLink { get; set; }

        [DataType(DataType.Url)]
        public string LinkedInLink { get; set; }

        [DataType(DataType.Url)]
        public string GithubLink { get; set; }

        [DataType(DataType.Url)]
        public string YoutubeLink { get; set; }
    }
}