using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Courses;

namespace ZaminEducation.Service.Interfaces
{
    public interface IYouTubeService
    {
        ValueTask<CourseVideo> CreateAsync(string link, long id);
        ValueTask<ICollection<CourseVideo>> CreateRangeAsync(string youtubePlaylist, long courseId, long? courseModuleId = null);
        ValueTask<bool> DeleteAsync(long youtubeId);
        ValueTask<bool> DeleteRangeAsync(long courseId);
        ValueTask<ICollection<CourseVideo>> UpdateRangeAsync(string youtubePlaylist, long courseId, long courseModuleId);
        ValueTask<CourseVideo> UpdateAsync(long videoId, string link);
        ValueTask<CourseVideo> GetAsync(Expression<Func<CourseVideo, bool>> expression);
        ValueTask<IEnumerable<CourseVideo>> GetAllAsync(PaginationParams @params,
            Expression<Func<CourseVideo, bool>> expression = null);
        ValueTask<IEnumerable<string>> GetLinksAsync(string playlistLink);
        ValueTask SetModuleIdAsync(long[] Ids, long courseId, long moduleId);
    }
}
