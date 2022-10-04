using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Api.Controllers
{
    public class DirectionController : BaseController
    {
        private readonly IDirectionService directionService;

        public DirectionController(IDirectionService directionService)
        {
            this.directionService = directionService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Direction>> CreateAsync(
            DirectionForCreationDto dto)
            => Ok(await directionService.CreateAsync(dto));

        [HttpDelete]
        public async ValueTask<ActionResult<bool>> DeleteAsync(long id)
            => Ok(await directionService.DeleteAsync(c => c.Id == id));

        [HttpGet("id")]
        public async ValueTask<ActionResult<Direction>> GetAsync(long id)
            => Ok(await directionService.GetAsync(c => c.Id == id));

        [HttpPut]
        public async ValueTask<ActionResult<Direction>> UpdateAsync(long id,
            DirectionForCreationDto dto)
            => Ok(await directionService.UpdateAsync(c => c.Id == id, dto));

        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<Direction>>> GetAllAsync(
            PaginationParams @params)
            => Ok(await directionService.GetAllAsync(@params));
    }
}
