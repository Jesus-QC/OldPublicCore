using System.Collections.Generic;
using Core.Features.Extensions;
using Core.Modules.Subclasses.Features;
using Core.Modules.Subclasses.Features.Enums;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace Core.Modules.Subclasses.Handlers;

public class PlayerHandler
{
    private readonly HashSet<Player> _undisguisedPlayers = new ();

    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (ev.NewRole is RoleType.Tutorial or RoleType.Spectator)
        {
            ev.Player.SetDisplaySubclass(null);
            ev.Player.CustomInfo = string.Empty;
            ev.Player.SetSubclass(null);

            if (ev.NewRole is RoleType.Spectator)
                ev.Player.UnDisguiseAll();
            
            return;
        }

        Subclass subclass = ev.NewRole.GetRandomSubclass();

        ev.Player.SetDisplaySubclass(subclass);
        foreach (Player player in ev.Player.CurrentSpectatingPlayers)
        {
            if(player.IsDead)
                player.SetDisplaySubclass(subclass);
        }
        
        if (subclass is null)
        {
            ev.Player.CustomInfo = string.Empty;
            ev.Player.SetSubclass(null);

            return;
        }
        
        ev.Player.SetSubclass(subclass);

        if (subclass.SpawnInventory is not null)
        {
            ev.Items.Clear();
            ev.Items.AddRange(subclass.SpawnInventory);
        }

        if (subclass.SpawnAmmo is not null)
        {
            ev.Ammo.Clear();

            foreach (KeyValuePair<ItemType, ushort> ammo in subclass.SpawnAmmo)
                ev.Ammo.Add(ammo.Key, ammo.Value);
        }

        if (_undisguisedPlayers.Contains(ev.Player))
            _undisguisedPlayers.Remove(ev.Player);
            
        if (ev.NewRole != subclass.SpawnAs && subclass.SpawnAs != RoleType.None)
            ev.NewRole = subclass.SpawnAs;

        if(!ev.Player.IsUsingStamina)
            ev.Player.IsUsingStamina = true;
    }

    public void OnSpawning(SpawningEventArgs ev)
    {
        if(ev.RoleType is RoleType.Tutorial or RoleType.Spectator)
            return;

        Subclass s = ev.Player.GetSubclass();
        if (s is null)
        {
            if(ev.Player.Scale != Vector3.one)
                ev.Player.Scale = Vector3.one;
            return;
        }

        if (s.Health > 0)
            ev.Player.Health = s.Health;
        if (s.Ahp > 0)
            ev.Player.AddAhp(s.Ahp, decay: 0);
        
        ev.Player.Broadcast(10, $"\n<b><color={s.SpawnAs.GetColor().ToHex()}>{s.Description}</color></b>", shouldClearPrevious: true);
        
        ev.Player.CustomInfo = $"<color=#50C878>{(s.Abilities.HasFlag(SubclassAbility.Disguised) ? "Default" : s.Name)}\n(Custom Subclass)</color>";

        ev.Player.SetDisplaySubclass(s);
        
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
        if(ev.Attacker is null || ev.Target is null || ev.Attacker == ev.Target || ev.Target.Role.Team == ev.Attacker.Role.Team)
            return;

        Subclass tS = ev.Target.GetSubclass();
        Subclass s = ev.Attacker.GetSubclass();
        
        if (tS is null || s is null)
            return;

        if (tS.Team == s.Team)
        {
            ev.Amount = 0;
            ev.IsAllowed = false;
            return;
        }
        
        if (ev.Handler.Type is DamageType.Explosion && tS.Abilities.HasFlag(SubclassAbility.GrenadeImmunity))
        {
            ev.Amount = 0;
            ev.IsAllowed = false;
            return;
        }
        
        ev.Amount *= s.DamageMultiplier;

        if (s.Abilities.HasFlag(SubclassAbility.Disguised) && !_undisguisedPlayers.Contains(ev.Attacker))
        {
            ev.Attacker.Broadcast(5, "\n<b>You have been discovered!</b>");
            _undisguisedPlayers.Add(ev.Attacker);
            ev.Attacker.ChangeAppearance(s.SpawnAs);
        }
    }

    public void OnDying(DyingEventArgs ev)
    {
        if(ev.Target is null)
            return;
        
        Subclass tS = ev.Target.GetSubclass();

        if (tS is null)
            return;
        
        if (ev.Handler.Type is DamageType.Explosion && tS.Abilities.HasFlag(SubclassAbility.GrenadeImmunity))
            ev.IsAllowed = false;
    }

    public void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
    {
        foreach (Player target in ev.TargetsToAffect.ToArray())
        {
            Subclass subclass = target.GetSubclass();
            if(subclass is null || !subclass.Abilities.HasFlag(SubclassAbility.GrenadeImmunity))
                return;

            ev.TargetsToAffect.Remove(target);
        }
    }
    
    public void OnChangingSpectatedPlayer(ChangingSpectatedPlayerEventArgs ev)
    {
        if(ev.Player == ev.NewTarget || ev.Player.IsAlive)
            return;
        
        ev.Player.SetDisplaySubclass(ev.NewTarget.GetSubclass());
    }
}