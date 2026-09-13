using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace StandardUnheardEdition;

public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.dragonx86.standard-unheard-edition";
    public string Name { get; init; } = "Standard Unheard Edition";
    public string Author { get; init; } = "DragonX86-dev";
    public List<string>? Contributors { get; init; }
    public Version Version { get; init; } = new("1.0.0");
    public Range SptVersion { get; init; } = new("~4.1.0");
    public bool HasPrepatcher { get; init; }
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, Range>? ModDependencies { get; init; }
    public string? Url { get; init; } = "https://github.com/dragonx86dev/SPT-StandardUnheardEdition";
    public string License { get; init; } = "MIT";
}