using Core.Features.Attribute;
using Core.Features.Data.Configs;
using Core.Loader.Features;

namespace Core.Modules.Pets;

[DisabledFeature]
public class PetsModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "Pets";

    public override void OnEnabled()
    {
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        base.OnDisabled();
    }
}