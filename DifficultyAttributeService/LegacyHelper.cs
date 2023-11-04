using System.Text.Json;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.IO.Serialization;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;

namespace DifficultyAttributeService;

public class LegacyHelper
{
    public static Ruleset GetRulesetFromLegacyId(int id)
    {
        return id switch
        {
            0 => new OsuRuleset(),
            1 => new TaikoRuleset(),
            2 => new CatchRuleset(),
            3 => new ManiaRuleset(),
            _ => throw new ArgumentException("Unknown ruleset id")
        };
    }
    public static Ruleset GetRulesetFromShortName(string name)
    {
        return name switch
        {
            "osu" => new OsuRuleset(),
            "taiko" => new TaikoRuleset(),
            "fruits" => new CatchRuleset(),
            "mania" => new ManiaRuleset(),
            _ => throw new ArgumentException("Unknown ruleset name")
        };
    }
    
    public static Ruleset GetRuleset(Beatmap beatmap, string? rulesetName, int? rulesetId)
    {
        if (rulesetId.HasValue)
            return GetRulesetFromLegacyId(rulesetId.Value);
        
        return rulesetName is not null ? GetRulesetFromShortName(rulesetName) : GetRulesetFromLegacyId(beatmap.BeatmapInfo.Ruleset.OnlineID);
    }

    public static IEnumerable<Mod> GetMods(Ruleset ruleset, JsonElement? modsUnsafe)
    {
        var modsList = new List<Mod>();

        if (!modsUnsafe.HasValue)
            return modsList;

        var mods = modsUnsafe.Value;
        
        if (mods.ValueKind == JsonValueKind.Number)
            modsList.AddRange(ruleset.ConvertFromLegacyMods((LegacyMods)mods.GetInt32()));

        if (mods.ValueKind != JsonValueKind.Array) return modsList;
        
        foreach (var item in mods.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.String)
            {
                var modAcronym = item.GetString() ?? string.Empty;
                var mod = ruleset.CreateModFromAcronym(modAcronym);
                if (mod != null)
                    modsList.Add(mod);
            }
            else
            {
                var apiMod = item.GetRawText().Deserialize<APIMod>();
                modsList.Add(apiMod.ToMod(ruleset));
            }
        }

        return modsList;
    }
}