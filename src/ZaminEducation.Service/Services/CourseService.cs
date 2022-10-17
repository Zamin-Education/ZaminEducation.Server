using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Helpers;
using ZaminEducation.Service.Interfaces;
using ZaminEducation.Service.Interfaces.Courses;
using ZaminEducation.Service.ViewModels;

namespace ZaminEducation.Service.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IYouTubeService youTubeService;
        private readonly IRepository<Course> courseRepository;
        private readonly IRepository<CourseRate> courseRateRepository;
        private readonly IRepository<ReferralLink> referralLinkRepository;
        private readonly IAttachmentService attachmentService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public CourseService(
            IRepository<Course> courseRepository,
            IYouTubeService youTubeService,
            IRepository<CourseRate> courseRateRepository,
            IAttachmentService attachmentService,
            IUserService userService,
            IMapper mapper,
            IRepository<ReferralLink> referralLinkRepository)
        {
            this.courseRepository = courseRepository;
            this.youTubeService = youTubeService;
            this.courseRateRepository = courseRateRepository;
            this.attachmentService = attachmentService;
            this.mapper = mapper;
            this.userService = userService;
            this.referralLinkRepository = referralLinkRepository;
        }

        public async ValueTask<Course> CreateAsync(CourseForCreationDto courseForCreationDto)
        {
            Course course = await courseRepository.GetAsync(expression: c =>
                c.YouTubePlaylistLink.Equals(courseForCreationDto.YouTubePlaylistLink));

            if (course is not null)
                throw new ZaminEducationException(400, "Course already exists");

            await IsAuthor(courseForCreationDto.AuthorId);

            long? attachmentId = null;
            if (courseForCreationDto.Image is not null)
            {
                var attachmentDto = courseForCreationDto.Image.ToAttachmentOrDefault();
                var attachment = await this.attachmentService.UploadAsync(dto: attachmentDto);
                attachmentId = attachment.Id;
            }

            Course mappedCourse = mapper.Map<Course>(source: courseForCreationDto);

            mappedCourse.ImageId = attachmentId;
            mappedCourse.Create();

            Course entity = await courseRepository.AddAsync(entity: mappedCourse);

            await courseRepository.SaveChangesAsync();

            entity.Videos = await youTubeService.CreateRangeAsync(
                youtubePlaylist: courseForCreationDto.YouTubePlaylistLink,
                courseId: entity.Id);

            return entity;
        }

        public async ValueTask<string> GenerateLinkAsync(long courseId)
        {
            await GetAsync(c => c.Id == courseId);

            var linkExists = await referralLinkRepository.GetAsync(
                l => l.CourseId == courseId &&
                l.UserId == (long)HttpContextHelper.UserId && l.State != Domain.Enums.ItemState.Deleted);


            if (linkExists is null)
            {
                string generatedLink = Guid.NewGuid().ToString("N").Substring(0, 9);

                ReferralLink link = new ReferralLink()
                {
                    CourseId = courseId,
                    UserId = (long)HttpContextHelper.UserId,
                    GeneratedLink = generatedLink
                };

                await referralLinkRepository.AddAsync(link);
                await referralLinkRepository.SaveChangesAsync();

                return generatedLink;
            }

            return linkExists.GeneratedLink;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<Course, bool>> expression)
        {
            Course course = await courseRepository.GetAsync(expression: expression);

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            courseRepository.Delete(entity: course);
            await this.attachmentService.DeleteAsync(a => a.Name == course.Image.Name);

            // DeleteAsync() ^ has SaveChanges inside, therefore it is not here

            //await youTubeService.DeleteRangeAsync(course.Id);

            return true;
        }

        public async ValueTask<IEnumerable<Course>> GetAllAsync(
            PaginationParams @params,
            Expression<Func<Course, bool>> expression = null,
            string search = null)
        {
            IQueryable<Course> pagedList = courseRepository.GetAll(
                expression: expression,
                includes: new string[] { "Author", "Category", "Image", "Rates" },
                isTracking: false)
                .ToPagedList(@params);

            return !string.IsNullOrEmpty(search)
                ? pagedList.Where(
                    c => c.Name == search ||
                    c.Author.FirstName == search ||
                    c.Author.LastName == search ||
                    c.Author.Username == search ||
                    c.Description.Contains(search) ||
                    c.Category.Name == search)
                : await pagedList.ToListAsync();
        }

        public async ValueTask<CourseViewModel> GetAsync(Expression<Func<Course, bool>> expression)
        {
            Course course = await courseRepository.GetAsync(
                expression: expression,
                includes: new string[] { "Category", "Image", "Modules", "Targets", "Rates", "Videos" });

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            course.ViewCount++;

            CourseViewModel courseView = mapper.Map<CourseViewModel>(source: course);

            courseView.Rate = CalculateRates(rates: course.Rates);

            await courseRepository.SaveChangesAsync();

            return courseView;
        }

        public async ValueTask<Course> UpdateAsync(
            Expression<Func<Course, bool>> expression,
            CourseForCreationDto courseForCreationDto)
        {
            Course course = await courseRepository.GetAsync(expression: expression);

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            long? attachmentId = null;
            if (courseForCreationDto.Image is not null)
            {
                var attachmentDto = courseForCreationDto.Image.ToAttachmentOrDefault();
                var attachment = await this.attachmentService.UpdateAsync(course.Id, courseForCreationDto.Image.OpenReadStream());
                attachmentId = attachment.Id;
            }
            course = mapper.Map(courseForCreationDto, course);

            course.ImageId = (long)attachmentId;
            course.Update();

            course = courseRepository.Update(entity: course);

            course.Videos = await this.youTubeService.CreateRangeAsync(
                youtubePlaylist: course.YouTubePlaylistLink,
                courseId: course.Id);

            return course;
        }

        public async ValueTask<CourseViewModel> AddAttachmentAsync(long courseId, AttachmentForCreationDto dto)
        {
            Course entity = await this.courseRepository.GetAsync(c => c.Id.Equals(courseId));

            if (entity is null)
                throw new ZaminEducationException(404, "Course not found");

            var attachment = await this.attachmentService.UploadAsync(dto);

            entity.ImageId = attachment.Id;
            entity.Update();

            this.courseRepository.Update(entity);

            await this.courseRepository.SaveChangesAsync();

            return this.mapper.Map<CourseViewModel>(entity);
        }

        public async ValueTask<IEnumerable<CourseTarget>> GetCourseTargetsAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Targets" });

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            return course.Targets;
        }

        public async ValueTask<IEnumerable<CourseVideo>> GetCourseVideosAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Videos" });

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            return course.Videos;
        }

        private double CalculateRates(IEnumerable<CourseRate> rates)
            => (double)rates.Sum(r => r.Value) / (double)rates.Count();

        public async Task<CourseRate> Rate(long id, byte value)
        {
            var course = await courseRepository.GetAsync(c => c.Id == id);

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            var courseRate = await courseRateRepository.GetAsync(
                                cr => cr.UserId == HttpContextHelper.UserId &&
                                      cr.CourseId == id);

            if (courseRate is not null)
            {
                courseRate.Value = value;
                courseRate.Update();

                await courseRateRepository.SaveChangesAsync();

                return courseRate;
            }

            var newCourseRate = await courseRateRepository.AddAsync(new CourseRate()
            {
                Value = value,
                CourseId = id,
                UserId = (long)HttpContextHelper.UserId,
                CreatedBy = HttpContextHelper.UserId
            });
            await courseRateRepository.SaveChangesAsync();

            return newCourseRate;
        }

        public async Task<CourseRate> GetCourseRateOfUser(long id)
        {
            var course = await courseRepository.GetAsync(c => c.Id == id);

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            var existCourseRate = await courseRateRepository.GetAsync(
                                    cr => cr.UserId == HttpContextHelper.UserId &&
                                           cr.CourseId == id);

            if (existCourseRate is null)
                throw new ZaminEducationException(404, "CourseRate not found");

            return existCourseRate;
        }

        public async ValueTask<Course> RetrieveAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await this.courseRepository.GetAsync(expression);

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            return course;
        }

        private async ValueTask IsAuthor(long authorId)
        {
            User author = await userService.GetAsync(u => u.Id == authorId);

            if (author is null)
                throw new ZaminEducationException(404, "Author not found");

            if (author.Role != UserRole.Mentor)
                throw new ZaminEducationException(400, "Not the Author");
        }
    }
}