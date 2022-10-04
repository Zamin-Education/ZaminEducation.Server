using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Users;

namespace ZaminEducation.Service.Interfaces
{
    public interface IUserAssetService
    {
        ValueTask<UserAsset> CreateAsync(long userId, long fileId);
        ValueTask<UserAsset> UpdateAsync(Expression<Func<UserAsset, bool>> expression, UserAsset userAsset);
        ValueTask<bool> DeleteAsync(Expression<Func<UserAsset, bool>> expression);
        ValueTask<UserAsset> GetAsync(Expression<Func<UserAsset, bool>> expression);
        ValueTask<IEnumerable<UserAsset>> GetAllAsync(PaginationParams @params, Expression<Func<UserAsset, bool>> expression);
    }
}
