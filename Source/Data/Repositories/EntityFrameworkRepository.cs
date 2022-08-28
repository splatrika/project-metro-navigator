using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Data.Repositories;

public abstract class EntityFrameworkRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : EntityBase, IAggregateRoot
    where TContext : DbContext
{
    protected readonly TContext _context;


    public EntityFrameworkRepository(TContext context)
    {
        _context = context;
    }


    public async Task AddAsync(TEntity instance)
    {
        await _context.AddAsync(instance);
    }


    public abstract Task DeleteAsync(int id);
    public abstract Task<TEntity> GetAsync(int id, bool trancked = true);
    public abstract Task<bool> ContainsAsync(int id);


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


    public Task UpdateAsync(TEntity instance)
    {
        _context.Update(instance);
        return Task.CompletedTask;
    }



    protected async Task<T> GetAsync<T>(int id, bool tracking,
        IQueryable<T> query) where T : EntityBase
    {
        if (!tracking)
        {
            query = query.AsNoTracking();
        }
        return await query.SingleAsync(x => x.Id == id);
    }
}

