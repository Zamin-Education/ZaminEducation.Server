using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.HomePage;

namespace ZaminEducation.Service.Interfaces
{
    public interface IHomePagesInfoService
    {
        ValueTask<HomePagesInfo> CreateAsync(HomePagesInfo entity);
        ValueTask<bool> DeleteAsync();
        ValueTask<HomePagesInfo> GetAsync();
    }
}
