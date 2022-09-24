using Core.Features.Data.Configs;
using Core.Loader.Features;

namespace Core.Modules.TrustedVote;

public class TrustedVoteModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "TrustedVote";
}