using System.ComponentModel.DataAnnotations;

namespace ZaminEducation.Service.DTOs.HomePage;

public class ReasonForCreationDto
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
}