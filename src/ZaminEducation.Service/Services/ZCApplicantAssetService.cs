using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class ZCApplicantAssetService : IZCApplicantAssetService
    {
        private readonly IRepository<ZCApplicantAsset> assetRepository;

        public ZCApplicantAssetService(IRepository<ZCApplicantAsset> assetRepository)
        {
            this.assetRepository = assetRepository;
        }

        public async ValueTask<ZCApplicantAsset> CreateAsync(long userId, long fileId)
        {
            var created = await assetRepository.AddAsync(new ZCApplicantAsset()
            {
                UserId = userId,
                FileId = fileId
            });
            await assetRepository.SaveChangesAsync();

            return created;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<ZCApplicantAsset, bool>> expression)
        {
            var exist = await GetAsync(expression);

            if (exist is null)
                return false;

            assetRepository.Delete(exist);
            await assetRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<ZCApplicantAsset>> GetAllAsync(PaginationParams @params,
            Expression<Func<ZCApplicantAsset, bool>> expression)
            => assetRepository.GetAll(expression).ToPagedList(@params);

        public async ValueTask<ZCApplicantAsset> GetAsync(Expression<Func<ZCApplicantAsset, bool>> expression)
            => await assetRepository.GetAsync(expression);

        public async ValueTask<ZCApplicantAsset> UpdateAsync(Expression<Func<ZCApplicantAsset, bool>> expression, ZCApplicantAsset userAsset)
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
