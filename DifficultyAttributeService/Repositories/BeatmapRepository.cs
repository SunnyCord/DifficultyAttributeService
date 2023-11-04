using StackExchange.Redis;
using osu.Game.Beatmaps;
using osu.Game.IO.Serialization;

namespace DifficultyAttributeService.Repositories;

public class BeatmapRepository : RedisRepositoryBase<Beatmap>
{
    private readonly IDatabase _redis;
    
    private static TimeSpan GetExpiry(BeatmapOnlineStatus status)
    {
        switch (status)
        {
            case BeatmapOnlineStatus.Ranked:
            case BeatmapOnlineStatus.Approved:
            case BeatmapOnlineStatus.Loved:
                return TimeSpan.FromDays(14);
            case BeatmapOnlineStatus.Qualified:
                return TimeSpan.FromDays(7);
            default:
                return TimeSpan.FromDays(1);
        }
    }
    
    public BeatmapRepository(IDatabase redis)
    {
        _redis = redis;
    }

    public override async Task<Beatmap?> GetByIdAsync(int id)
    {
        var beatmap = await _redis.StringGetAsync(GetCacheKey(id));
        return beatmap.HasValue ? beatmap.ToString().Deserialize<Beatmap>() : null;
    }

    public override async Task AddAsync(Beatmap obj)
    {
        TimeSpan expiry = GetExpiry(obj.BeatmapInfo.Status);
        await _redis.StringSetAsync(GetCacheKey(obj.BeatmapInfo.OnlineID), obj.Serialize(), expiry);
    }
    
    public override async Task UpdateAsync(Beatmap obj)
    {
        TimeSpan expiry = GetExpiry(obj.BeatmapInfo.Status);
        await _redis.StringSetAsync(GetCacheKey(obj.BeatmapInfo.OnlineID), obj.Serialize(), expiry);
    }
    
    public override async Task DeleteAsync(Beatmap obj)
    {
        await _redis.KeyDeleteAsync(GetCacheKey(obj.BeatmapInfo.OnlineID));
    }
}