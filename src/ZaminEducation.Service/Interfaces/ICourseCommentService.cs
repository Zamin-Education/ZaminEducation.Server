using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.UserCourses;

namespace ZaminEducation.Service.Interfaces
{
    public interface ICourseCommentService
    {
        ValueTask<CourseComment> AddAsync(long courseId, string message, long? parentId = null);
        ValueTask<bool> DeleteAsync(long id);
        ValueTask<IEnumerable<CourseComment>> GetAllAsync(PaginationParams @params, long courseId);
        ValueTask<CourseComment> GetAsync(long id);
        ValueTask<CourseComment> UpdateAsync(long id, string message);
        ValueTask<IEnumerable<CourseComment>> GetReplies(long Id);
        ValueTask<IEnumerable<CourseComment>> SearchAsync(PaginationParams @params, string search);
    }
}