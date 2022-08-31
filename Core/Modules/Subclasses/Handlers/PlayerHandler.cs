using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Modules.Subclasses.Features.Enums;
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
            ev.Player.CustomInfo = string.Empty;
            ev.Player.SetSubclass(null);
            return;
        }

        var subclass = ev.NewRole.GetRandomSubclass();

        if (subclass is null)
        {
            ev.Player.ClearHint(ScreenZone.TopBar);
            ev.Player.ClearHint(ScreenZone.TopBarSecondary);
            ev.Player.CustomInfo = string.Empty;
            ev.Player.SetSubclass(null);
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

        if (ev.NewRole != subclass.SpawnAs && subclass.SpawnAs != RoleType.None)
            ev.NewRole = subclass.SpawnAs;
    }

    public void OnSpawning(SpawningEventArgs ev)
    {
        if(ev.RoleType is RoleType.Tutorial or RoleType.Spectator)
            return;

        var s = ev.Player.GetSubclass();
        if (s is null)
        {
            if(ev.Player.Scale != Vector3.one)
                ev.Player.Scale = Vector3.one;
            return;
        }

        if (s.Health > 0)
            ev.Player.Health = s.Health;
        if (s.Ahp > 0)
            ev.Player.AddAhp(s.Ahp);
        
        ev.Player.Broadcast(10, $"\n<b><color={s.SpawnAs.GetColor().ToHex()}>{s.Description}</color></b>", shouldClearPrevious: true);
        
        ev.Player.CustomInfo = $"<color=#50C878>{(s.Abilities.HasFlag(SubclassAbility.Disguised) ? "Default" : s.Name)}\n(Custom Subclass)</color>";
        
        if (s.Scale != Vector3.one)
            ev.Player.Scale = s.Scale;
        else if(ev.Player.Scale != Vector3.one)
            ev.Player.Scale = Vector3.one;

        if (s.SpawnLocation != RoleType.None)
            ev.Position = s.SpawnLocation.GetRandomSpawnProperties().Item1;
        
        s.OnSpawning(ev.Player);
    }

    public void OnHurting(HurtingEventArgs ev)
    {
        if(ev.Attacker is null || ev.Target is null || ev.Attacker == ev.Target)
            return;

        var tS = ev.Target.GetSubclass();

        if (tS is not null)
        {
            if (tS.Abilities.HasFlag(SubclassAbility.Disguised))
            {
                ev.Target.ChangeAppearance(tS.SpawnAs);
                ev.Target.Broadcast(5, "\n<b>You have been discovered!</b>");
            }
        }
        
        var s = ev.Attacker.GetSubclass();
        if(s is null)
            return;

        ev.Amount *= s.DamageMultiplier;
    }

    public void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
    {
        foreach (var target in ev.TargetsToAffect.ToArray())
        {
            var subclass = target.GetSubclass();
            if(subclass is null || !subclass.Abilities.HasFlag(SubclassAbility.GrenadeImmunity))
                return;

            ev.TargetsToAffect.Remove(target);
        }
    }
}