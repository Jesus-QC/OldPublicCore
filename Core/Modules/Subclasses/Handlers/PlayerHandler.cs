using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace Core.Modules.Subclasses.Handlers;

public class PlayerHandler
{
    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (ev.NewRole is RoleType.Tutorial or RoleType.Spectator)
        {
            ev.Player.ClearHint(ScreenZone.TopBar);
            ev.Player.ClearHint(ScreenZone.TopBarSecondary);
            return;
        }

        var subclass = ev.NewRole.GetRandomSubclass();

        if (subclass is null)
        {
            ev.Player.ClearHint(ScreenZone.TopBar);
            ev.Player.ClearHint(ScreenZone.TopBarSecondary);
            return;
        }
        
        ev.Player.SetSubclass(subclass);

        ev.Player.SendHint(ScreenZone.TopBar,
            subclass.Color is null
                ? $"subclass: {subclass.Name} ({subclass.Rarity.ToString().ToLower()})"
                : $"subclass: <color={subclass.Color}>{subclass.Name} ({subclass.Rarity.ToString().ToLower()})</color>");

        ev.Player.SendHint(ScreenZone.TopBarSecondary, "abilities: " + subclass.Abilities.ToString().ToLower());

        if (subclass.SpawnInventory is not null)
        {
            ev.Items.Clear();
            ev.Items.AddRange(subclass.SpawnInventory);
        }

        if (subclass.SpawnAmmo is not null)
        {
            ev.Ammo.Clear();

            foreach (var ammo in subclass.SpawnAmmo)
                ev.Ammo.Add(ammo.Key, ammo.Value);
        }
    }

    public void OnSpawning(SpawningEventArgs ev)
    {
        if(ev.RoleType is RoleType.Tutorial or RoleType.Spectator)
            return;

        var s = ev.Player.GetSubclass();
        if(s is null)
            return;

        if (s.Health > 0)
            ev.Player.Health = s.Health;
        if (s.Ahp > 0)
            ev.Player.AddAhp(s.Ahp);
        
        ev.Player.Broadcast(10, "\n<b>" + s.Description + "</b>", shouldClearPrevious: true);

        if (s.Scale != Vector3.one)
            ev.Player.Scale = s.Scale;
            
        s.OnSpawning(ev.Player);
    }

    public void OnHurting(HurtingEventArgs ev)
    {
        if(ev.Attacker is null || ev.Target is null || ev.Attacker == ev.Target)
            return;
        
        var s = ev.Attacker.GetSubclass();
        if(s is null)
            return;

        ev.Amount *= s.DamageMultiplier;
    }
}