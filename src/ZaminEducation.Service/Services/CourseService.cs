using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> courseRepository;

        public CourseService(IRepository<Course> courserRepository)
        {
            this.courseRepository = courserRepository;
        }

        public async Task<IEnumerable<CourseModule>> GetCourseModulesAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Modules" });

            return course is not null 
                ? course.Modules 
                : throw new ZaminEducationException(404, "Course not found");
        }

        public async Task<IEnumerable<CourseTarget>> GetCourseTargetsAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Targets" });

            return course is not null 
                ? course.Targets 
                : throw new ZaminEducationException(404, "Course not found");
        }

        public async Task<IEnumerable<CourseVideo>> GetCourseVideosAsync(Expression<Func<Course, bool>> expression)
        {
            var course = await courseRepository.GetAsync(expression, new string[] { "Videos" });

            return course is not null 
                ? course.Videos
                : throw new ZaminEducationException(404, "Course not found");
        }
    }
}