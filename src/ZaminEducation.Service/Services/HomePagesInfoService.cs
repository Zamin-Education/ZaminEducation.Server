using ZaminEducation.Domain.Entities.HomePage;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class HomePagesInfoService : IHomePagesInfoService
    {
        public async ValueTask<HomePagesInfo> CreateAsync(HomePagesInfo entity)
            => await FileHelper.SaveHomePagesInfoAsync(entity);

        public async ValueTask<bool> DeleteAsync()
            => FileHelper.RemoveHomePageInfo();

        public async ValueTask<HomePagesInfo> GetAsync()
            => FileHelper.GetHomePagesInfo();
    }
}
