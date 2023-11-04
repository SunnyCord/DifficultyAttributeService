namespace DifficultyAttributeService.Repositories;

public abstract class RedisRepositoryBase<T>
{
    protected string GetCacheKey(int id)
    {
        return $"cache:{typeof(T).Name.ToLower()}:{id}";
    }
    
    public abstract Task<T?> GetByIdAsync(int id);
    public abstract Task AddAsync(T obj);
    public abstract Task UpdateAsync(T obj);
    public abstract Task DeleteAsync(T obj);
}