using Microsoft.AspNetCore.Http;
using ZaminEducation.Domain.Entities.MainPages;
using ZaminEducation.Service.DTOs.HomePage;

namespace ZaminEducation.Service.Interfaces;

public interface IHomePageService
{
    ValueTask<HomePage> GetHomePageAsync();
    ValueTask<bool> UpdateHeaderAsync(
       long id,
       HomePageHeaderForCreationDto dto);

    ValueTask<bool> UpdateProjectAboutInfoAsync(
       long id,
       InfoAboutProjectForCreationDto dto);

    ValueTask<bool> UpdateOpportinutyAsync(
       long id,
       OfferedOpportunitesForCreationDto dto);

    ValueTask<bool> UpdateReasonAsync(long id, ReasonForCreationDto dto);

    ValueTask<bool> UpdateSocialNetwordAsync(long id, SocialNetworksForCreationDto dto);

    ValueTask<bool> UpdatePhotoGalleryAsync(long id, IFormFile dto);
}