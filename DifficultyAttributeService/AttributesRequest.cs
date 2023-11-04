using System.Text.Json;
using System.Text.Json.Serialization;

namespace DifficultyAttributeService;

public class AttributesRequest
{
    public JsonElement Mods { get; set; }
    
    public string? Ruleset { get; set; }
    
    [JsonPropertyName("ruleset_id")]
    public int? RulesetId { get; set; }
}