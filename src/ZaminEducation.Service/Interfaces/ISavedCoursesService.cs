using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.UserCourses;
using ZaminEducation.Service.DTOs.UserCourses;

namespace ZaminEducation.Service.Interfaces
{
    public interface ISavedCoursesService
    {
        ValueTask<SavedCourse> GetAsync(Expression<Func<SavedCourse, bool>> expression = null);
        ValueTask<bool> ToggleAsync(SavedCourseForCreationDto dto);
        ValueTask<IEnumerable<SavedCourse>> GetAllAsync(PaginationParams @params, Expression<Func<SavedCourse, bool>> expression = null, string search = null);
    }
}