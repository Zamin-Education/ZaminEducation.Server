using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ZaminEducation.Service.DTOs.HomePage;

public class InfoAboutProjectForCreationDto
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public IFormFile FormFile { get; set; }
}