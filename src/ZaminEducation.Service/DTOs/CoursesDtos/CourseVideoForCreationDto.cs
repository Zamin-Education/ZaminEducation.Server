using System.ComponentModel.DataAnnotations;

namespace ZaminEducation.Service.DTOs.CoursesDtos
{
    public class CourseVideoForCreationDto
    {
        [Required, DataType(DataType.Url)]
        public string Link { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        public long Length { get; set; }

        public long CourseModuleId { get; set; }

        public long CourseId { get; set; }
    }
}
