using AutoMapper;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class ApplicantUserService : IApplicantUserService
    {
        private readonly IRepository<ApplicantUser> userRepository;
        private readonly IRepository<UserAsset> userAssetRepository;
        private readonly IAttachmentService attachmentService;
        private readonly IRepository<Direction> directionRepository;
        private readonly IMapper mapper;

        public ApplicantUserService(IRepository<ApplicantUser> userRepository, 
            IRepository<Direction> directionRepository,
            IAttachmentService attachmentService,
            IMapper mapper,IRepository<UserAsset> repository)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userAssetRepository = repository;
            this.directionRepository = directionRepository;
            this.attachmentService = attachmentService;
        }

        public async ValueTask<ApplicantUser> CreateAsync(ApplicantUserForCreationDto dto,
                            Stream stream = null, string fileName = null)
        {
            var category = await directionRepository.GetAsync(c => c.Id == dto.CategoryId);

            if (category is null)
                throw new Exception("Category is not found!!!");

            var user = mapper.Map<ApplicantUser>(dto);
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

                await userAssetRepository.AddAsync(new UserAsset()
                {
                    UserId = user.Id,
                    FileId = attachment.Id
                });
            }
            await userRepository.SaveChangesAsync();

            return user;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<ApplicantUser, bool>> expression)
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

        public async ValueTask<IEnumerable<ApplicantUser>> GetAllAsync(PaginationParams @params)
            => userRepository.GetAll().ToPagedList(@params);

        public async ValueTask<ApplicantUser> GetAsync(Expression<Func<ApplicantUser, bool>> expression)
            => await userRepository.GetAsync(expression);

        public async ValueTask<ApplicantUser> UpdateAsync(Expression<Func<ApplicantUser, bool>> expression,
            ApplicantUserForCreationDto dto)
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
    }
}
