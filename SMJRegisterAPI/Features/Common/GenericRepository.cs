using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Common.Entities;
using SMJRegisterAPI.Database.Contexts;

namespace SMJRegisterAPI.Features.Common;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
    where T : BaseEntity
{

    public virtual async  Task<T> AddAsync(T entity)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual  async Task UpdateAsync(T entity, int id)
    {
        var entry = await context.Set<T>().FindAsync(id != null); 
        context.Entry(entry).CurrentValues.SetValues(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        await context.SaveChangesAsync();
    }

    public virtual async Task<List<T>> GetAllAsync() => await context.Set<T>().ToListAsync(); 

    public virtual  async Task<T> GetByIdAsync(int id)
    {
        if (await context.Set<T>().FindAsync(id) != null) 
            await context.Set<T>().FindAsync(id);

        return null;
    }
}