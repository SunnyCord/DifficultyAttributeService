using DifficultyAttributeService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public async Task<IActionResult> Get(int id)
    {
        var beatmap = await _beatmapService.GetOrCreateAsync(id);
        return Content(beatmap.Serialize(), "application/json");
    }
    
    [HttpPost("{id}/attributes")]
    public async Task<IActionResult> Post(int id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] AttributesRequest? request)
    {
        var beatmap = await _beatmapService.GetOrCreateAsync(id);
        var attributes = await DifficultyService.CalculateAsync(beatmap, request?.Mods, request?.Ruleset, request?.RulesetId);
        return Content(attributes.Serialize(), "application/json");
    }
}