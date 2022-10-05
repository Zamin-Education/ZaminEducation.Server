using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.MainPages;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.DTOs.HomePage;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services;

public class HomePageService : IHomePageService
{
    private readonly IRepository<HomePage> homePageRepository;
    private readonly IAttachmentService attachmentService;
    private readonly IRepository<HomePageHeader> homePageHeaderRepository;
    private readonly IRepository<Course> courseRepository;
    private readonly IRepository<OfferedOpportunities> offeredOpportunitiesRepository;
    private readonly IRepository<InfoAboutProject> infoAboutProjectRepository;
    private readonly IRepository<Reason> reasonRepository;
    private readonly IRepository<SocialNetworks> socialNetworksRepository;
    private readonly IRepository<PhotoGalleryAttachment> photoGalleryAttachmentRepository;
    private readonly IMapper mapper;

    public HomePageService(IMapper mapper,
        IRepository<HomePage> homePageRepository,
        IRepository<HomePageHeader> homePageHeaderRepository,
        IAttachmentService attachmentService,
        IRepository<Course> courseRepostitory,
        IRepository<OfferedOpportunities> offeredOpportunitiesRepository,
        IRepository<InfoAboutProject> infoAboutProjectRepository,
        IRepository<Reason> reasonRepository,
        IRepository<SocialNetworks> socialNetworksRepository,
        IRepository<PhotoGalleryAttachment> photoGalleryAttachmentRepository)
    {
        this.homePageRepository = homePageRepository;
        this.homePageHeaderRepository = homePageHeaderRepository;
        this.attachmentService = attachmentService;
        courseRepository = courseRepostitory;
        this.mapper = mapper;
        this.offeredOpportunitiesRepository = offeredOpportunitiesRepository;
        this.infoAboutProjectRepository = infoAboutProjectRepository;
        this.reasonRepository = reasonRepository;
        this.socialNetworksRepository = socialNetworksRepository;
        this.photoGalleryAttachmentRepository = photoGalleryAttachmentRepository;
    }

    public async ValueTask<HomePage> GetHomePageAsync()
    {
        string[] includes = { "PhotoGallery" };

        var homePage = homePageRepository.GetAll()
            .Include(hp => hp.HomePageHeader)
            .ThenInclude(hph => hph.Image)
            .Include(hp => hp.InfoAboutProject)
            .ThenInclude(i => i.Image)
            .Include(hp => hp.OpportunitiesOffered)
            .ThenInclude(o => o.Attachment)
            .Include(hp => hp.PhotoGallery)
            .ThenInclude(pg => pg.Photos)
            .FirstOrDefault();

        homePage.PopularCourses = await GetPopularCoursesAsync();

        return homePage;
    }

    public async ValueTask<bool> UpdateHeaderAsync(long id, HomePageHeaderForCreationDto dto)
    {
        var existHeader = await homePageHeaderRepository.GetAsync(h => h.Id == id);
        if (existHeader is null)
            throw new ZaminEducationException(404, "Home pages header not found.");

        Attachment attachment;
        if (dto.FormFile is not null)
        {
            var attachmentDto = dto.FormFile.ToAttachmentOrDefault();
            attachment = await attachmentService.UploadAsync(attachmentDto);

            existHeader.ImageId = attachment.Id;
        }

        existHeader = mapper.Map(dto, existHeader);
        existHeader.Update();

        await homePageHeaderRepository.SaveChangesAsync();

        return existHeader is not null;
    }

    public async ValueTask<bool> UpdateOpportinutyAsync(long id,
        OfferedOpportunitesForCreationDto dto)
    {
        var existOpportunity =
            await offeredOpportunitiesRepository.GetAsync(h => h.Id == id);
        if (existOpportunity is null)
            throw new ZaminEducationException(404, "This offered opportunity is not found.");

        Attachment attachment;
        if (dto.FormFile is not null)
        {
            var attachmentDto = dto.FormFile.ToAttachmentOrDefault();
            attachment = await attachmentService.UploadAsync(attachmentDto);

            existOpportunity.AttachmentId = attachment.Id;
        }
        existOpportunity = mapper.Map(dto, existOpportunity);
        existOpportunity.Update();

        await homePageHeaderRepository.SaveChangesAsync();

        return existOpportunity is not null;
    }

    public async ValueTask<bool> UpdatePhotoGalleryAsync(long id, IFormFile dto)
    {
        var existAttachment =
            await photoGalleryAttachmentRepository.GetAsync(h => h.Id == id);
        if (existAttachment is null)
            throw new ZaminEducationException(404, "Picture not found.");

        if (dto is null)
            throw new ZaminEducationException(400, "Image now uploaded.");

        var attachmentDto = dto.ToAttachmentOrDefault();
        var attachment = await attachmentService.UpdateAsync(id, attachmentDto.Stream);

        return existAttachment is not null;
    }

    public async ValueTask<bool> UpdateProjectAboutInfoAsync(long id,
        InfoAboutProjectForCreationDto dto)
    {
        var existInfo =
            await infoAboutProjectRepository.GetAsync(h => h.Id == id);
        if (existInfo is null)
            throw new ZaminEducationException(404, "Information not found.");

        Attachment attachment;
        AttachmentForCreationDto attachmentDto = null;
        if (existInfo.ImageId != 0)
        {
            attachmentDto = dto.FormFile.ToAttachmentOrDefault();
            attachment =
                await attachmentService.UpdateAsync(existInfo.ImageId, attachmentDto.Stream);
        }
        else
            attachment = await attachmentService.UploadAsync(attachmentDto);

        existInfo.ImageId = attachment.Id;
        existInfo = mapper.Map(dto, existInfo);
        existInfo.Update();
        await infoAboutProjectRepository.SaveChangesAsync();

        return existInfo is not null;
    }

    public async ValueTask<bool> UpdateReasonAsync(long id, ReasonForCreationDto dto)
    {
        var existReason = await reasonRepository.GetAsync(r => r.Id == id);
        if (existReason is null)
            throw new ZaminEducationException(404, "data not found.");

        existReason = mapper.Map(dto, existReason);
        existReason.Update();
        await reasonRepository.SaveChangesAsync();

        return existReason is not null;
    }

    public async ValueTask<bool> UpdateSocialNetwordAsync(long id, SocialNetworksForCreationDto dto)
    {
        var existSocialNetwork =
            await socialNetworksRepository.GetAsync(sn => sn.Id == id);
        if (existSocialNetwork is null)
            throw new ZaminEducationException(404, "networks not found.");

        existSocialNetwork = mapper.Map(dto, existSocialNetwork);
        existSocialNetwork.Update();
        await reasonRepository.SaveChangesAsync();

        return existSocialNetwork is not null;
    }

    private async ValueTask<IEnumerable<Course>> GetPopularCoursesAsync()
    {
        return await courseRepository.GetAll()
            .OrderBy(c => c.ViewCount)
                .Take(6)
                    .ToListAsync();
    }
}