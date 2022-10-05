using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ZaminEducation.Service.DTOs.HomePage;

public class OfferedOpportunitesForCreationDto
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }

    public IFormFile FormFile { get; set; }
}