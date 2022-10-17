using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using ZaminEducation.Data.DbContexts;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Data.Repositories;
using ZaminEducation.Domain.Entities.Commons;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Interfaces;
using ZaminEducation.Service.Interfaces.Courses;
using ZaminEducation.Service.Mappers;
using ZaminEducation.Service.Services;
using ZaminEducation.Service.Services.Courses;

namespace ZaminEducation.Test.Unit.Services.YouTube
{
    public partial class YoutubeServiceAndCourseServiceTest
    {
        private readonly ZaminEducationDbContext zaminEducationDbContext;

        private readonly IRepository<Course> courseRepositoryMock;
        private readonly IRepository<User> userRepositoryMock;
        private readonly IRepository<CourseCategory> courseCategoryRepositoryMock;
        private readonly IRepository<CourseVideo> courseVideoRepositoryMock;
        private readonly IRepository<CourseRate> courseRateRepositoryMock;
        private readonly IRepository<Attachment> attachmentReositoryMock;
        private readonly IRepository<CourseModule> courseModuleRepositoryMock;
        private readonly IRepository<ReferralLink> referralLinkRepositoryMock;

        private readonly ICourseCategoryService courseCategoryService;
        private readonly IYouTubeService youTubeService;
        private readonly IUserService userService;
        private readonly ICourseService courseService;
        private readonly IAttachmentService attachmentService;
        private readonly ICourseModuleService courseModuleService;
        private readonly IMapper mapper;

        public YoutubeServiceAndCourseServiceTest()
        {
            var options = new DbContextOptionsBuilder<ZaminEducationDbContext>()
                           .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            mapper = new MapperConfiguration(
                mapper => mapper.AddProfile<MappingProfile>()).CreateMapper();

            zaminEducationDbContext = new ZaminEducationDbContext(options);

            courseCategoryRepositoryMock = new Repository<CourseCategory>(zaminEducationDbContext);
            userRepositoryMock = new Repository<User>(zaminEducationDbContext);
            courseModuleRepositoryMock = new Repository<CourseModule>(zaminEducationDbContext);
            courseRepositoryMock = new Repository<Course>(zaminEducationDbContext);
            courseVideoRepositoryMock = new Repository<CourseVideo>(zaminEducationDbContext);
            attachmentReositoryMock = new Repository<Attachment>(zaminEducationDbContext);
            referralLinkRepositoryMock = new Repository<ReferralLink>(zaminEducationDbContext);
            courseRateRepositoryMock = new Repository<CourseRate>(zaminEducationDbContext);

            courseCategoryService = new CourseCategoryService(mapper, courseCategoryRepositoryMock);
            attachmentService = new AttachmentService(attachmentReositoryMock);
            userService = new UserService(userRepositoryMock, mapper, attachmentService);
            courseModuleService = new CourseModuleService(courseModuleRepositoryMock, courseService, mapper);
            youTubeService = new YouTubeService(courseVideoRepositoryMock, courseRepositoryMock);
            courseService = new CourseService(courseRepositoryMock,
                youTubeService,
                courseRateRepositoryMock,
                attachmentService,
                userService,
                mapper,
                referralLinkRepositoryMock);
        }

        private CourseForCreationDto CreateRandomCourse(CourseForCreationDto courseForCreationDto)
        {
            courseForCreationDto.YouTubePlaylistLink = "https://www.youtube.com/watch?v=2NwXtLaZJkQ&list=PLAXSS6gGBPcVrqEaXc1Av4CLaLxbv9HOx";
            courseForCreationDto.Description = Faker.Lorem.Sentence(3);
            courseForCreationDto.Name = Faker.Name.First();
            courseForCreationDto.Level = (CourseLevel)Faker.RandomNumber.Next(0, 2);
            courseForCreationDto.Description = Faker.Lorem.Sentence(5);
            //courseForCreationDto.ModuleName = Faker.Name.First();
            
            return courseForCreationDto;
        }

        private CourseCategoryForCreationDto CreateRandomCategory(CourseCategoryForCreationDto courseCategoryForCreationDto)
        {
            courseCategoryForCreationDto.Name = Faker.Lorem.Sentence(2);
            return courseCategoryForCreationDto;
        }
        private UserForCreationDto CreateRandomAuthor(UserForCreationDto userForCreationDto)
        {
            userForCreationDto.FirstName = Faker.Name.First();
            userForCreationDto.LastName = Faker.Name.Last();
            userForCreationDto.Username = Faker.Name.Middle();
            userForCreationDto.AddressId = null;
            userForCreationDto.Gender = (Gender)Faker.RandomNumber.Next(0, 1);
            userForCreationDto.Bio = Faker.Lorem.Sentence(1);
            userForCreationDto.DateOfBirth = DateTime.UtcNow;
            userForCreationDto.Password = Faker.Lorem.Sentence(1);

            return userForCreationDto;
        }
    }
}

