using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;

namespace DifficultyAttributeService;

public class LegacyHelper
{
    public static Ruleset GetRulesetFromLegacyID(int id)
    {
        switch (id)
        {
            case 0:
                return new OsuRuleset();
            case 1:
                return new TaikoRuleset();
            case 2:
                return new CatchRuleset();
            case 3:
                return new ManiaRuleset();
            default:
                throw new ArgumentException("Unknown ruleset id");
            
        }
    }
    public static Ruleset GetRulesetFromShortName(string name)
    {
        switch (name)
        {
            case "osu":
                return new OsuRuleset();
            case "taiko":
                return new TaikoRuleset();
            case "fruits":
                return new CatchRuleset();
            case "mania":
                return new ManiaRuleset();
            default:
                throw new ArgumentException("Unknown ruleset name");
        }
    }
}