using Microsoft.AspNetCore.Mvc;
using ZaminEducation.Domain.Entities.HomePage;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Api.Controllers
{
    public class HomePagesInfoController : BaseController
    {
        private readonly IHomePagesInfoService homePagesInfoService;

        public HomePagesInfoController(IHomePagesInfoService homePagesInfoService)
        {
            this.homePagesInfoService = homePagesInfoService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<HomePagesInfo>> CreateAsync(HomePagesInfo entity)
            => Ok(await homePagesInfoService.CreateAsync(entity));

        [HttpDelete]
        public async ValueTask<ActionResult<bool>> DeleteAsync()
            => Ok(await homePagesInfoService.DeleteAsync());

        [HttpGet]
        public async ValueTask<ActionResult<HomePagesInfo>> GetAsync()
            => Ok(await homePagesInfoService.GetAsync());

        [HttpPut]
        public async ValueTask<ActionResult<HomePagesInfo>> UpdateAsync(HomePagesInfo entity)
            => Ok(await homePagesInfoService.CreateAsync(entity));
    }
}
