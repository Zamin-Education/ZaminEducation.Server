using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Service.DTOs.Users;

namespace ZaminEducation.Service.Interfaces
{
    public interface IZCDirectionService
    {
        ValueTask<ZCApplicantDirection> CreateAsync(ZCDirectionForCreationDto dto);
        ValueTask<ZCApplicantDirection> UpdateAsync(Expression<Func<ZCApplicantDirection, bool>> expression,
            ZCDirectionForCreationDto dto);
        ValueTask<bool> DeleteAsync(Expression<Func<ZCApplicantDirection, bool>> expression);
        ValueTask<ZCApplicantDirection> GetAsync(Expression<Func<ZCApplicantDirection, bool>> expression);
        ValueTask<IEnumerable<ZCApplicantDirection>> GetAllAsync(PaginationParams @params);
    }
}
