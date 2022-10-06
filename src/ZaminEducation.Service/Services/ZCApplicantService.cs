using AutoMapper;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.HomePage;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class ZCApplicantService : IZCApplicantService
    {
        private readonly IRepository<ZCApplicant> userRepository;
        private readonly IRepository<ZCApplicantAsset> userAssetRepository;
        private readonly IAttachmentService attachmentService;
        private readonly IRepository<ZCApplicantDirection> directionRepository;
        private readonly IMapper mapper;

        public ZCApplicantService(IRepository<ZCApplicant> userRepository, 
            IRepository<ZCApplicantDirection> directionRepository,
            IAttachmentService attachmentService,
            IMapper mapper,IRepository<ZCApplicantAsset> repository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userAssetRepository = repository;
            this.directionRepository = directionRepository;
            this.attachmentService = attachmentService;
        }

        public async ValueTask<ZCApplicant> CreateAsync(ZCApplicantForCreationDto dto,
                            Stream stream = null, string fileName = null)
        {
            var category = await directionRepository.GetAsync(c => c.Id == dto.CategoryId);

            if (category is null)
                throw new Exception("Category is not found!!!");

            var user = mapper.Map<ZCApplicant>(dto);
            user.Directory = category;
            user.Create();

            user = await userRepository.AddAsync(user);

            if (stream is not null)
            {
                var file = await FileHelper.SaveAsync(new AttachmentForCreationDto()
                {
                    Stream = stream,
                    FileName = fileName
                });
                var attachment = await attachmentService.CreateAsync(file.fileName, file.filePath);

                await userAssetRepository.AddAsync(new ZCApplicantAsset()
                {
                    UserId = user.Id,
                    FileId = attachment.Id
                });
            }
            await userRepository.SaveChangesAsync();

            return user;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<ZCApplicant, bool>> expression)
        {
            var user = await GetAsync(expression);

            if (user is null)
                return false;

            var asset = await userAssetRepository.GetAsync(c => c.UserId == user.Id);

            if (asset is not null)
            {
                userAssetRepository.Delete(asset);
                await attachmentService.DeleteAsync(a => a.Id == asset.FileId);
            }

            userRepository.Delete(user);
            await userRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<ZCApplicant>> GetAllAsync(PaginationParams @params)
            => userRepository.GetAll().ToPagedList(@params);

        public async ValueTask<ZCApplicant> GetAsync(Expression<Func<ZCApplicant, bool>> expression)
            => await userRepository.GetAsync(expression);

        public async ValueTask<ZCApplicant> UpdateAsync(Expression<Func<ZCApplicant, bool>> expression,
            ZCApplicantForCreationDto dto)
        {
            var user = await GetAsync(expression);
            var categor = await directionRepository.GetAsync(c => c.Id == dto.CategoryId);

            if (user is null || categor is null)
                throw new Exception("Bad request!!!");

            user = mapper.Map(dto, user);
            user.Directory = categor;
            user.Update();

            user = userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            return user;
        }

        public async ValueTask<ZCApplicationInfo> CreateHomePageInfoAsync(ZCApplicationInfo entity)
            => await FileHelper.SaveHomePagesInfoAsync(entity);

        public async ValueTask<bool> DeleteHomePageInfoAsyncAsync()
            => FileHelper.RemoveHomePageInfo();

        public async ValueTask<ZCApplicationInfo> GetHomePageInfoAsyncAsync()
            => FileHelper.GetHomePagesInfo();
    }
}
