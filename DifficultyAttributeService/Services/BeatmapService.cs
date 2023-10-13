using DifficultyAttributeService.Repositories;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Online.API;

namespace DifficultyAttributeService.Services;

public class BeatmapService
{
    private readonly RepositoryBase<Beatmap> _beatmapRepository;
    
    public BeatmapService(RepositoryBase<Beatmap> beatmapRepository)
    {
        _beatmapRepository = beatmapRepository;
    }
    
    public Beatmap? GetById(int id) => _beatmapRepository.GetById(id);
    
    public void Add(Beatmap obj) => _beatmapRepository.Add(obj);
    
    public void Update(Beatmap obj) => _beatmapRepository.Update(obj);
    
    public void Delete(Beatmap obj) => _beatmapRepository.Delete(obj);

    public Beatmap GetOrCreate(int id)
    {
        var beatmap = GetById(id);
        if (beatmap == null)
        {
            using var req = new OsuWebRequest($"https://osu.ppy.sh/osu/{id}");
            req.Perform();

            using var reader = new LineBufferedReader(req.ResponseStream);
            beatmap = new LegacyBeatmapDecoder().Decode(reader);
            Add(beatmap);
        }
        return beatmap;
    }
}