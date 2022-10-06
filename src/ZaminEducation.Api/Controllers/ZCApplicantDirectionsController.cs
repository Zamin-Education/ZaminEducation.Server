using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Api.Controllers
{
    public class ZCApplicantDirectionsController : BaseController
    {
        private readonly IZCDirectionService directionService;

        public ZCApplicantDirectionsController(IZCDirectionService directionService)
        {
            this.directionService = directionService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<ZCApplicantDirection>> CreateAsync(
            ZCDirectionForCreationDto dto)
            => Ok(await directionService.CreateAsync(dto));

        [HttpDelete]
        public async ValueTask<ActionResult<bool>> DeleteAsync(long id)
            => Ok(await directionService.DeleteAsync(c => c.Id == id));

        [HttpGet("id")]
        public async ValueTask<ActionResult<ZCApplicantDirection>> GetAsync(long id)
            => Ok(await directionService.GetAsync(c => c.Id == id));

        [HttpPut]
        public async ValueTask<ActionResult<ZCApplicantDirection>> UpdateAsync(long id,
            ZCDirectionForCreationDto dto)
            => Ok(await directionService.UpdateAsync(c => c.Id == id, dto));

        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<ZCApplicantDirection>>> GetAllAsync(
            PaginationParams @params)
            => Ok(await directionService.GetAllAsync(@params));
    }
}
