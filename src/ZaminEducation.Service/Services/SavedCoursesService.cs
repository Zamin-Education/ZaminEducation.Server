using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Service.DTOs.UserCourses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class SavedCoursesService : ISavedCoursesService
    {
        private readonly IRepository<SavedCourse> savedCourseRepository;
        private readonly IMapper mapper;

        public SavedCoursesService(IRepository<SavedCourse> savedCourseRepository, IMapper mapper)
        {
            this.savedCourseRepository = savedCourseRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Agar course save qilingan bo'lsa SavedCourse dan o'chirib tashlab false qaytaradi 
        /// save qilinmagan bo'lsa save qilib true qaytaradi.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async ValueTask<bool> AddRemoveAsync(SavedCourseForCreationDto dto)
        {
            var result = await savedCourseRepository.GetAsync(p => p.CourseId == dto.CourseId);

            if (result is not null)
            {
                savedCourseRepository.Delete(result);
                await savedCourseRepository.SaveChangesAsync();
                return false;
            }

            var newSavedCourse = mapper.Map<SavedCourse>(dto);
            newSavedCourse = await savedCourseRepository.AddAsync(newSavedCourse);
            await savedCourseRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<SavedCourse>> GetAllAsync(
            PaginationParams @params, 
            Expression<Func<SavedCourse, bool>> expression = null)
        {
            var pagedList =
                savedCourseRepository.GetAll(expression, new string[] { "Course", "User" }, false).ToPagedList(@params);

            return await pagedList.ToListAsync();
        }

        public async ValueTask<IEnumerable<SavedCourse>> GetAllAsync(PaginationParams @params, string search) =>
            await savedCourseRepository.GetAll(includes: new string[] { "Course" },
                expression: sc => sc.Id.ToString() == search
                || sc.Course.Name == search)?
                .ToPagedList(@params).ToListAsync();

        public async ValueTask<SavedCourse> GetAsync(Expression<Func<SavedCourse, bool>> expression = null)
        {
            var result = await savedCourseRepository.GetAsync(expression, new string[] { "Course", "User" });

            if (result is null)
                throw new ZaminEducationException(404, "Saved course not found");

            return result;
        }
    }
}