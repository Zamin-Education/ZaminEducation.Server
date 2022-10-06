using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Users;

namespace ZaminEducation.Service.Interfaces
{
    public interface IZCApplicantAssetService
    {
        ValueTask<ZCApplicantAsset> CreateAsync(long userId, long fileId);
        ValueTask<ZCApplicantAsset> UpdateAsync(Expression<Func<ZCApplicantAsset, bool>> expression, ZCApplicantAsset userAsset);
        ValueTask<bool> DeleteAsync(Expression<Func<ZCApplicantAsset, bool>> expression);
        ValueTask<ZCApplicantAsset> GetAsync(Expression<Func<ZCApplicantAsset, bool>> expression);
        ValueTask<IEnumerable<ZCApplicantAsset>> GetAllAsync(PaginationParams @params, Expression<Func<ZCApplicantAsset, bool>> expression);
    }
}
