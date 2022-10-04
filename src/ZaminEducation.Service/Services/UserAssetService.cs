using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class UserAssetService : IUserAssetService
    {
        private readonly IRepository<UserAsset> assetRepository;

        public UserAssetService(IRepository<UserAsset> assetRepository)
        {
            this.assetRepository = assetRepository;
        }

        public async ValueTask<UserAsset> CreateAsync(long userId, long fileId)
        {
            var created = await assetRepository.AddAsync(new UserAsset()
            {
                UserId = userId,
                FileId = fileId
            });
            await assetRepository.SaveChangesAsync();

            return created;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<UserAsset, bool>> expression)
        {
            var exist = await GetAsync(expression);

            if (exist is null)
                return false;

            assetRepository.Delete(exist);
            await assetRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<UserAsset>> GetAllAsync(PaginationParams @params,
            Expression<Func<UserAsset, bool>> expression)
            => assetRepository.GetAll(expression).ToPagedList(@params);

        public async ValueTask<UserAsset> GetAsync(Expression<Func<UserAsset, bool>> expression)
            => await assetRepository.GetAsync(expression);

        public async ValueTask<UserAsset> UpdateAsync(Expression<Func<UserAsset, bool>> expression, UserAsset userAsset)
        {
            var exist = await GetAsync(expression);

            if (exist is null)
                throw new Exception("Not found");

            exist.Update();
            exist.UserId = userAsset.UserId;
            exist.FileId = userAsset.FileId;

            exist = assetRepository.Update(exist);
            await assetRepository.SaveChangesAsync();

            return exist;
        }
    }
}
