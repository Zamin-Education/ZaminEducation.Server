using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZaminEducation.Data.DbContexts;
using ZaminEducation.Data.IRepositories;

namespace ZaminEducation.Data.Repositories;

public class Repository<TSource> : IRepository<TSource>
    where TSource : class
{
    protected readonly ZaminEducationDbContext _dbContext;
    protected DbSet<TSource> _dbSet;

    public Repository(ZaminEducationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TSource>();
    }

    public async Task<TSource> AddAsync(TSource entity)
    {
        var entityEntry = await _dbSet.AddAsync(entity);

        return entityEntry.Entity;
    }

    public void Delete(Expression<Func<TSource, bool>> expression)
    {
        var entity = _dbSet.FirstOrDefault(expression);

        _dbSet.Remove(entity);
    }

    public IQueryable<TSource> GetAll(Expression<Func<TSource, bool>> expression = null, string include = null, bool isTracking = true)
    {
        IQueryable<TSource> query = expression is null ? _dbSet : _dbSet.Where(expression);
        
        if(include is not null)
            query = query.Include(include);

        if (!isTracking)
            query = query.AsNoTracking();

        return query;
    }

    public async Task<TSource> GetAsync(Expression<Func<TSource, bool>> expression, string include = null)
    {
        if (include is not null)
            return _dbSet.Where(expression).Include(include).FirstOrDefault();

        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    public TSource Update(TSource entity)
    {
        var entryEntity = _dbSet.Update(entity);

        return entryEntity.Entity;
    }
    

    public async Task SavaChangesAsync()
       => await _dbContext.SaveChangesAsync();
}
