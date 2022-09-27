using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features;

public interface IAbility
{
    public SubclassAbility Ability { get; set; }
    public int Cooldown { get; set; }
    public void OnUsing(Player player);
}