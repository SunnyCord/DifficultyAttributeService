using DifficultyAttributeService.Repositories;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Online.API;

namespace DifficultyAttributeService.Services;

public class BeatmapService
{
    private readonly RedisRepositoryBase<Beatmap> _beatmapRepository;
    
    public BeatmapService(RedisRepositoryBase<Beatmap> beatmapRepository)
    {
        _beatmapRepository = beatmapRepository;
    }
    
    public async Task<Beatmap?> GetByIdAsync(int id) => await _beatmapRepository.GetByIdAsync(id);
    
    public async Task AddAsync(Beatmap obj) => await _beatmapRepository.AddAsync(obj);
    
    public async Task UpdateAsync(Beatmap obj) => await _beatmapRepository.UpdateAsync(obj);
    
    public async Task DeleteAsync(Beatmap obj) => await _beatmapRepository.DeleteAsync(obj);

    public async Task<Beatmap> GetOrCreate(int id)
    {
        var beatmap = await GetByIdAsync(id);
        if (beatmap == null)
        {
            using var req = new OsuWebRequest($"https://osu.ppy.sh/osu/{id}");
            await req.PerformAsync();

            using var reader = new LineBufferedReader(req.ResponseStream);
            beatmap = new LegacyBeatmapDecoder().Decode(reader);
            AddAsync(beatmap);
        }
        return beatmap;
    }
}