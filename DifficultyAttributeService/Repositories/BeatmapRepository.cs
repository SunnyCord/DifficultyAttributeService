using StackExchange.Redis;
using osu.Game.Beatmaps;
using osu.Game.IO.Serialization;

namespace DifficultyAttributeService.Repositories;

public class BeatmapRepository : RepositoryBase<Beatmap>
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

    public override Beatmap? GetById(int id)
    {
        var beatmap = _redis.StringGet(GetCacheKey(id));
        return beatmap.HasValue ? beatmap.ToString().Deserialize<Beatmap>() : null;
    }

    public override void Add(Beatmap obj)
    {
        TimeSpan expiry = GetExpiry(obj.BeatmapInfo.Status);
        _redis.StringSet(GetCacheKey(obj.BeatmapInfo.OnlineID), obj.Serialize(), expiry);
    }
    
    public override void Update(Beatmap obj)
    {
        TimeSpan expiry = GetExpiry(obj.BeatmapInfo.Status);
        _redis.StringSet(GetCacheKey(obj.BeatmapInfo.OnlineID), obj.Serialize(), expiry);
    }
    
    public override void Delete(Beatmap obj)
    {
        _redis.KeyDelete(GetCacheKey(obj.BeatmapInfo.OnlineID));
    }
}