using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Service.DTOs.Courses;

namespace ZaminEducation.Service.Interfaces
{
    public interface ICourseCategoryService
    {
        ValueTask<CourseCategory> CreateAsync(CourseCategoryForCreationDto courseCategoryForCreationDto);
        ValueTask<CourseCategory> UpdateAsync(long id, CourseCategoryForCreationDto courseCategoryForCreationDto);
        ValueTask<bool> DeleteAsync(Expression<Func<CourseCategory, bool>> expression);
        ValueTask<IEnumerable<CourseCategory>> GetAllAsync(PaginationParams @params, Expression<Func<CourseCategory, bool>> expression = null);
        ValueTask<CourseCategory> GetAsync(Expression<Func<CourseCategory, bool>> expression);

    }
}
