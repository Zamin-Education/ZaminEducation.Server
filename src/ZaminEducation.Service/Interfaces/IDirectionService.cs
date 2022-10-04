using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Service.DTOs.Users;

namespace ZaminEducation.Service.Interfaces
{
    public interface IDirectionService
    {
        ValueTask<Direction> CreateAsync(DirectionForCreationDto dto);
        ValueTask<Direction> UpdateAsync(Expression<Func<Direction, bool>> expression,
            DirectionForCreationDto dto);
        ValueTask<bool> DeleteAsync(Expression<Func<Direction, bool>> expression);
        ValueTask<Direction> GetAsync(Expression<Func<Direction, bool>> expression);
        ValueTask<IEnumerable<Direction>> GetAllAsync(PaginationParams @params);
    }
}
