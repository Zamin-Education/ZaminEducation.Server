using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZaminEducation.Data.IRepositories;
using ZaminEducation.Domain.Configurations;
using ZaminEducation.Domain.Entities.Courses;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.DTOs.Courses;
using ZaminEducation.Service.Exceptions;
using ZaminEducation.Service.Extensions;
using ZaminEducation.Service.Interfaces;

namespace ZaminEducation.Service.Services
{
    public class CourseCategoryService : ICourseCategoryService
    {
        private readonly IMapper mapper;
        private readonly IRepository<CourseCategory> categoryRepository;

        public CourseCategoryService(IMapper mapper, IRepository<CourseCategory> categoryRepository)
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async ValueTask<CourseCategory> CreateAsync(CourseCategoryForCreationDto courseCategoryForCreationDto)
        {
            var result = await categoryRepository.GetAsync(x => x.Name == courseCategoryForCreationDto.Name);

            if (result is not null)
                throw new ZaminEducationException(400, "Course category already exists");

            CourseCategory mappedCategory = mapper.Map<CourseCategory>(courseCategoryForCreationDto);
            mappedCategory.Create();
            mappedCategory = await categoryRepository.AddAsync(mappedCategory);

            await categoryRepository.SaveChangesAsync();

            return mappedCategory;
        }

        public async ValueTask<bool> DeleteAsync(Expression<Func<CourseCategory, bool>> expression)
        {
            var result = await categoryRepository.GetAsync(expression);
            if (result is null)
                throw new ZaminEducationException(404, "Course category not found");

            categoryRepository.Delete(result);
            await categoryRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<IEnumerable<CourseCategory>> GetAllAsync(PaginationParams @params, Expression<Func<CourseCategory, bool>> expression = null)
        {
            var courseCategory = categoryRepository.GetAll(expression, isTracking: false);

            return await courseCategory.ToPagedList(@params).ToListAsync();
        }

        public async ValueTask<CourseCategory> GetAsync(Expression<Func<CourseCategory, bool>> expression)
        {
            var result = await categoryRepository.GetAsync(expression, new string[] { "Courses" });

            if (result is null)
                throw new ZaminEducationException(404, "Course category not found");

            return result;
        }

        public async ValueTask<CourseCategory> UpdateAsync(long id, CourseCategoryForCreationDto courseCategoryForCreationDto)
        {
            var existCategory = await categoryRepository.GetAsync(c => c.Id == id && c.State != ItemState.Deleted);

            if (existCategory is null)
                throw new ZaminEducationException(404, "Course category not found");

            existCategory = mapper.Map(courseCategoryForCreationDto, existCategory);

            existCategory.Update();

            categoryRepository.Update(existCategory);

            await categoryRepository.SaveChangesAsync();

            return existCategory;
        }
    }
}
