namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<bool> ContainsAsync(int id);
    Task<T> GetAsync(int id, bool tracking = true);
    Task AddAsync(T instance);
    Task UpdateAsync(T instance);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}

