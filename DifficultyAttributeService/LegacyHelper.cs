using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
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
}