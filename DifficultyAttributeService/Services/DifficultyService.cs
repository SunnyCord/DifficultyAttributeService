using System.Text.Json;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
namespace DifficultyAttributeService.Services;

public class DifficultyService
{
    public static async Task<DifficultyAttributes> CalculateAsync(Beatmap beatmap, JsonElement? mods, string? rulesetName, int? rulesetId)
    {
        var ruleset = LegacyHelper.GetRuleset(beatmap, rulesetName, rulesetId);
        var modsList = LegacyHelper.GetMods(ruleset, mods);
        return await Task.Run(() => ruleset.CreateDifficultyCalculator(new FlatWorkingBeatmap(beatmap)).Calculate(modsList));
    }
}