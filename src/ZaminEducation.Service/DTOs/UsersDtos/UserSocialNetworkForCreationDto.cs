using System.ComponentModel.DataAnnotations;

namespace ZaminEducation.Service.DTOs.UsersDtos
{
    public class UserSocialNetworkForCreationDto
    {
        public long UserId { get; set; }

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
