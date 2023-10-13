using DifficultyAttributeService.Services;
using Microsoft.AspNetCore.Mvc;
using osu.Game.Beatmaps;
using osu.Game.IO.Serialization;

namespace DifficultyAttributeService.Controllers;

[ApiController]
[Route("/api/v2/beatmaps")]
public class BeatmapController : ControllerBase
{
    private readonly BeatmapService _beatmapService;
    
    public BeatmapController(BeatmapService beatmapService)
    {
        _beatmapService = beatmapService;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var beatmap = _beatmapService.GetOrCreate(id);
        return Ok(beatmap.Serialize());
    }
    
    [HttpPost("{id}/attributes")]
    public IActionResult Post(int id, [FromBody] AttributesRequest request)
    {
        var beatmap = _beatmapService.GetOrCreate(id);
        var ruleset = request.GetRuleset(beatmap)!;
        var mods = request.GetMods(ruleset);
        var attributes = ruleset.CreateDifficultyCalculator(new FlatWorkingBeatmap(beatmap)).Calculate(mods);
        return Ok(attributes.Serialize());
    }
}