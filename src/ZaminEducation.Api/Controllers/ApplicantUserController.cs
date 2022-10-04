using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Api.Controllers;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Interfaces;

namespace ZaminCreative.Api.Controllers
{
    public class ApplicantUserController : BaseController
    {
        private readonly IApplicantUserService applicantUserService;

        public ApplicantUserController(IApplicantUserService userService)
        {
            this.applicantUserService = userService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<ApplicantUser>> CreateAsync(
            [FromForm] ApplicantUserForCreationDto dto, IFormFile file = null)
            => Ok(await applicantUserService.CreateAsync(dto,file?.OpenReadStream(),file?.FileName));

        [HttpDelete]
        public async ValueTask<ActionResult<bool>> DeleteAsync(long id)
            => Ok(await applicantUserService.DeleteAsync(u => u.Id == id));

        [HttpPut]
        public async ValueTask<ActionResult<ApplicantUser>> UpdateAsync(
            long id,ApplicantUserForCreationDto dto)
            => Ok(await applicantUserService.UpdateAsync(u => u.Id == id, dto));

        [HttpGet("id")]
        public async ValueTask<ActionResult<ApplicantUser>> GetAsync(long id)
            => Ok(await applicantUserService.GetAsync(u => u.Id == id));

        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<ApplicantUser>>> GetAllAsync([FromQuery]PaginationParams @params) 
            => Ok(await applicantUserService.GetAllAsync(@params));
    }
}
