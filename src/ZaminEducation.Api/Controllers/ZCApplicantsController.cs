using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Api.Controllers;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.HomePage;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Interfaces;

namespace ZaminCreative.Api.Controllers
{
    public class ZCApplicantsController : BaseController
    {
        private readonly IZCApplicantService applicantUserService;


        public ZCApplicantsController(IZCApplicantService userService)
        {
            this.applicantUserService = userService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<ZCApplicant>> CreateAsync(
            [FromForm] ZCApplicantForCreationDto dto, IFormFile file = null)
            => Ok(await applicantUserService.CreateAsync(dto, file?.OpenReadStream(), file?.FileName));

        [HttpDelete]
        public async ValueTask<ActionResult<bool>> DeleteAsync(long id)
            => Ok(await applicantUserService.DeleteAsync(u => u.Id == id));

        [HttpPut]
        public async ValueTask<ActionResult<ZCApplicant>> UpdateAsync(
            long id, ZCApplicantForCreationDto dto)
            => Ok(await applicantUserService.UpdateAsync(u => u.Id == id, dto));

        [HttpGet("id")]
        public async ValueTask<ActionResult<ZCApplicant>> GetAsync(long id)
            => Ok(await applicantUserService.GetAsync(u => u.Id == id));

        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<ZCApplicant>>> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(await applicantUserService.GetAllAsync(@params));

        ///

        [HttpPost("page-info")]
        public async ValueTask<ActionResult<ZCApplicationInfo>> CreateHomePagesInfoAsync(ZCApplicationInfo entity)
            => Ok(await applicantUserService.CreateHomePageInfoAsync(entity));

        [HttpDelete("page-info")]
        public async ValueTask<ActionResult<bool>> DeleteHomePageInfoAsyncAsync()
            => Ok(await applicantUserService.DeleteHomePageInfoAsyncAsync());

        [HttpGet("page-info")]
        public async ValueTask<ActionResult<ZCApplicationInfo>> GetHomePageInfoAsyncAsync()
            => Ok(await applicantUserService.GetHomePageInfoAsyncAsync());
    }
}
