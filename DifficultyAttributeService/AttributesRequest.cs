using System.Text.Json;
using System.Text.Json.Serialization;
using osu.Game.Beatmaps.Legacy;
using osu.Game.IO.Serialization;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;

namespace DifficultyAttributeService;

public class AttributesRequest
{
    public JsonElement Mods { get; set; }
    
    public string? Ruleset { get; set; }
    
    [JsonPropertyName("ruleset_id")]
    public int? RulesetId { get; set; }

    public Ruleset? GetRuleset()
    {
        if (RulesetId.HasValue)
            return LegacyHelper.GetRulesetFromLegacyID(RulesetId.Value);
        if (Ruleset != null)
            return LegacyHelper.GetRulesetFromShortName(Ruleset);
        
        return null;
    }

    public IEnumerable<Mod> GetMods(Ruleset ruleset)
    {
        var  mods = new List<Mod>();

        if (Mods.ValueKind == JsonValueKind.Number)
            mods.AddRange(ruleset.ConvertFromLegacyMods((LegacyMods)Mods.GetInt32()));

        if (Mods.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in Mods.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                {
                    var modAcronym = item.GetString() ?? string.Empty;
                    var mod = ruleset.CreateModFromAcronym(modAcronym);
                    if (mod != null)
                        mods.Add(mod);
                }
                else
                {
                    var apiMod = item.GetRawText().Deserialize<APIMod>();
                    mods.Add(apiMod.ToMod(ruleset));
                }
            }
        }
        
        return mods;
    }
}