using AutoMapper;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.user;
using ZaminEducation.Service.DTOs.Users;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class DirectionService : IDirectionService
    {
        private readonly IRepository<Direction> directionRepository;
        private readonly IMapper mapper;

        public DirectionService(IRepository<Direction> directionRepo, IMapper mapper)
        {
            this.directionRepository = directionRepo;
            this.mapper = mapper;
        }

        public async ValueTask<Direction> CreateAsync(DirectionForCreationDto dto)
        {
            var exist = await GetAsync(c => c.Category == dto.Category);

            if (exist is not null)
                throw new Exception("This category alredy exist!!!");

            exist = mapper.Map<Direction>(dto);
            exist.Create();

            exist = await directionRepository.AddAsync(exist);
            await directionRepository.SaveChangesAsync();

            return exist;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<Direction, bool>> expression)
        {
            var exist = await GetAsync(expression);

            if (exist is null)
                return false;

            directionRepository.Delete(exist);
            await directionRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<Direction>> GetAllAsync(PaginationParams @params)
            => directionRepository.GetAll().ToPagedList(@params);

        public ValueTask<Direction> GetAsync(Expression<Func<Direction, bool>> expression)
            => directionRepository.GetAsync(expression);

        public async ValueTask<Direction> UpdateAsync(Expression<Func<Direction, bool>> expression,
            DirectionForCreationDto dto)
        {
            var exist = await GetAsync(expression);

            if (exist is null)
                throw new Exception("Not found");

            exist = mapper.Map(dto, exist);
            exist.Update();

            exist = directionRepository.Update(exist);
            await directionRepository.SaveChangesAsync();

            return exist;
        }
    }
}
