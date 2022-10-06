using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.HomePage;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Users;

namespace ZaminEducation.Service.Interfaces
{
    public interface IZCApplicantService
    {
        ValueTask<ZCApplicant> CreateAsync(ZCApplicantForCreationDto user,
            Stream stream = null, string fileName = null);
        ValueTask<ZCApplicant> UpdateAsync(Expression<Func<ZCApplicant, bool>> expression,
            ZCApplicantForCreationDto dto);
        ValueTask<bool> DeleteAsync(Expression<Func<ZCApplicant, bool>> expression);
        ValueTask<ZCApplicant> GetAsync(Expression<Func<ZCApplicant, bool>> expression);
        ValueTask<IEnumerable<ZCApplicant>> GetAllAsync(PaginationParams @params);
        ValueTask<ZCApplicationInfo> CreateHomePageInfoAsync(ZCApplicationInfo entity);
        ValueTask<bool> DeleteHomePageInfoAsyncAsync();
        ValueTask<ZCApplicationInfo> GetHomePageInfoAsyncAsync();
    }
}
