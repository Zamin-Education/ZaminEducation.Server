using System.Linq.Expressions;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Users;
using ZaminEducation.Service.DTOs.Users;

namespace ZaminEducation.Service.Interfaces
{
    public interface IApplicantUserService
    {
        ValueTask<ApplicantUser> CreateAsync(ApplicantUserForCreationDto user,
            Stream stream = null, string fileName = null);
        ValueTask<ApplicantUser> UpdateAsync(Expression<Func<ApplicantUser, bool>> expression,
            ApplicantUserForCreationDto dto);
        ValueTask<bool> DeleteAsync(Expression<Func<ApplicantUser, bool>> expression);
        ValueTask<ApplicantUser> GetAsync(Expression<Func<ApplicantUser, bool>> expression);
        ValueTask<IEnumerable<ApplicantUser>> GetAllAsync(PaginationParams @params);
    }
}
