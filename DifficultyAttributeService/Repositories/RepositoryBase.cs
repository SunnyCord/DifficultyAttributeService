namespace DifficultyAttributeService.Repositories;

public abstract class RepositoryBase<T>
{
    protected string GetCacheKey(int id)
    {
        return $"cache:{typeof(T).Name.ToLower()}:{id}";
    }
    
    public abstract T? GetById(int id);
    public abstract void Add(T obj);
    public abstract void Update(T obj);
    public abstract void Delete(T obj);
}