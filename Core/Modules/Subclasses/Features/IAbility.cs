using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features;

public interface IAbility
{
    public SubclassAbility Ability { get; set; }
    public uint Cooldown { get; set; }
    public bool OnUsing(Player player);
}