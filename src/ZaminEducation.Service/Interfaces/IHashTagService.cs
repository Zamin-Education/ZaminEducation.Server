using System.Linq.Expressions;
using ZaminEducation.Domain.Entities.Courses;

namespace ZaminEducation.Service.Interfaces
{
    public interface IHashTagService
    {
        ValueTask<IEnumerable<HashTag>> GetAll(Expression<Func<HashTag, bool>> expression);
    }
}
