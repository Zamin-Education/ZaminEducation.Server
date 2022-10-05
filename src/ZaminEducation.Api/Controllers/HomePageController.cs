using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Domain.Entities.MainPages;
using ZaminEducation.Service.DTOs.HomePage;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Api.Controllers
{
    public class HomePageController : BaseController
    {
        private readonly IHomePageService _homePageService;

        public HomePageController(IHomePageService homePageService)
        {
            _homePageService = homePageService;
        }

        /// <summary>
        /// take a landing page informations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async ValueTask<ActionResult<HomePage>> GetHomePageAsync()
        {
            return Ok(await _homePageService.GetHomePageAsync());
        }

        /// <summary>
        /// update main page header data 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("header/{id}")]
        public async ValueTask<ActionResult<bool>> UpdateHomePageHeaderAsync(
            [FromRoute] long id,
            [FromForm] HomePageHeaderForCreationDto dto)
        {
            return Ok(await _homePageService.UpdateHeaderAsync(id, dto));
        }

        /// <summary>
        /// Update opportunity title data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("opportunity/{id}")]
        public async ValueTask<ActionResult<bool>> UpdateOpportityAsync(
            [FromRoute] long id,
            [FromForm] OfferedOpportunitesForCreationDto dto)
        {
            return Ok(await _homePageService.UpdateOpportinutyAsync(id, dto));
        }

        /// <summary>
        /// Update photo gallery section images
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut("attachment/{id}")]
        public async ValueTask<ActionResult<bool>> UpdateGalleryImagesAsync(
            [FromRoute] long id, [FromForm] IFormFile file)
        {
            return Ok(await _homePageService.UpdatePhotoGalleryAsync(id, file));
        }

        /// <summary>
        /// update about us page data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("about-us/{id}")]
        public async ValueTask<ActionResult<bool>> UpdateProjectAboutInfoAsync(
            [FromRoute] long id,
            [FromForm] InfoAboutProjectForCreationDto dto)
        {
            return Ok(await _homePageService.UpdateProjectAboutInfoAsync(id, dto));
        }

        /// <summary>
        /// update opportunity reasons  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("opportunity-reason/{id}")]
        public async ValueTask<ActionResult<bool>> UpdateOpportunityReasonAsync(
            [FromRoute] long id, ReasonForCreationDto dto)
        {
            return Ok(await _homePageService.UpdateReasonAsync(id, dto));
        }

        /// <summary>
        /// update social networks 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("social-network/{id}")]
        public async ValueTask<ActionResult<bool>> UpdateSocialNetworksAsync(
            [FromRoute] long id, SocialNetworksForCreationDto dto)
        {
            return Ok(await _homePageService.UpdateSocialNetwordAsync(id, dto));
        }
    }
}
