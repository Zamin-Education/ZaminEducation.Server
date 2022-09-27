using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class HashTagService : IHashTagService
    {
        private readonly IRepository<HashTag> hashTagRepository;

        public HashTagService(IRepository<HashTag> hashTagRepository)
        {
            this.hashTagRepository = hashTagRepository;
        }

        public async ValueTask<IEnumerable<HashTag>> GetAll(Expression<Func<HashTag, bool>> expression) =>
            await hashTagRepository.GetAll(expression, isTracking: false).ToListAsync();
    }
}
