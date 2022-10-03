using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;
using ZaminEducation.Service.Interfaces.Courses;
using ZaminEducation.Service.ViewModels;

namespace ZaminEducation.Service.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> courseRepository;
        private readonly IYouTubeService youTubeService;
        private readonly IMapper mapper;

        public CourseService(
            IRepository<Course> courseRepository,
            IYouTubeService youTubeService,
            IMapper mapper)
        {
            this.courseRepository = courseRepository;
            this.youTubeService = youTubeService;
            this.mapper = mapper;
        }

        public async ValueTask<Course> CreateAsync(CourseForCreationDto courseForCreationDto)
        {
            Course course = await courseRepository.GetAsync(c =>
                c.YouTubePlaylistLink.Equals(courseForCreationDto.YouTubePlaylistLink));

            if (course is not null)
                throw new ZaminEducationException(400, "Course already exists");

            Course mappedCourse = mapper.Map<Course>(source: courseForCreationDto);

            Course entity = await courseRepository.AddAsync(entity: mappedCourse);

            await courseRepository.SaveChangesAsync();

            entity.Videos = (ICollection<CourseVideo>)await youTubeService.CreateRangeAsync(
                youtubePlaylist: courseForCreationDto.YouTubePlaylistLink,
                courseId: entity.Id);

            return entity;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<Course, bool>> expression)
        {
            Course course = await courseRepository.GetAsync(expression: expression);

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            courseRepository.Delete(entity: course);

            await courseRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<Course>> GetAllAsync(
            Expression<Func<Course, bool>> expression = null,
            PaginationParams @params = null)
        {
            IQueryable<Course> pagedList = courseRepository.GetAll(
                expression: expression,
                includes: new string[] { "Author", "Category", "Image", "Rates" },
                isTracking: false)
                .ToPagedList(@params);

            return await pagedList.ToListAsync();
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

            course = mapper.Map(courseForCreationDto, course);
            course.Update();

            course = courseRepository.Update(entity: course);
            await courseRepository.SaveChangesAsync();

            return course;
        }

        public async Task<IEnumerable<CourseModule>> GetCourseModulesAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Modules" });

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            return course.Modules;
        }

        public async Task<IEnumerable<CourseTarget>> GetCourseTargetsAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Targets" });

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            return course.Targets;
        }

        public async Task<IEnumerable<CourseVideo>> GetCourseVideosAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Videos" });

            if (course is null)
                throw new ZaminEducationException(404, "Course not found");

            return course.Videos;
        }

        public async ValueTask<IEnumerable<Course>> GetAllAsync(
            PaginationParams @params, string search) =>
                await courseRepository.GetAll(c => c.Id.ToString() == search || c.Name == search)?
                    .ToPagedList(@params).ToListAsync();

        private double CalculateRates(IEnumerable<CourseRate> rates)
            => (double)rates.Sum(r => r.Value) / (double)rates.Count();
    }
}