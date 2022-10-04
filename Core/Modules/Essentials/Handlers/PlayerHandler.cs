using System.Linq;
using Core.Modules.Essentials.Extensions;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Scp914;
using UnityEngine;

namespace Core.Modules.Essentials.Handlers;

public class PlayerHandler
{
    public void OnVerified(VerifiedEventArgs ev)
    {
        if (ev.Player.HasIllegalName())
            ev.Player.Kick("You have been kicked\nPlease don't advertise any website in your name.\n[Kicked by a Plugin]");
    }

    public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
    {
        if (ev.Player.Inventory.UserInventory.Items.Any(x => EssentialsModule.PluginConfig.ItemsThatDisablesTesla.Contains(x.Value.ItemTypeId)))
            ev.IsTriggerable = false;
    }

    public void OnHurting(HurtingEventArgs ev)
    {
        if(ev.Attacker == null)
            return;
            
        if (!EssentialsModule.PluginConfig.CanCuffedPlayersBeDamaged && ev.Target.IsCuffed && ev.Target != ev.Attacker && ev.Attacker.IsHuman)
            ev.IsAllowed = false;
    }

    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (ev.Items.Count == 8 || ev.NewRole.GetSide() == Side.Scp)
            return;
        
        ev.Items.Add(ItemType.Flashlight);
    }

    public void OnDied(DiedEventArgs ev)
    {
        if (Player.Get(ev.TargetOldRole.GetTeam()) is not Player[] { Length: 1 } players)
            return;
        
        foreach (Player player in players)
        {
            player.Broadcast(10, "\n<b>You're the <color=#ff4545>last one</color> on your team, don't disappoint others.</b>");
        }
    }

    public void OnLeft(LeftEventArgs ev)
    {
        if (Player.Get(ev.Player.Role.Team) is not Player[] { Length: 2 } players)
            return;
        
        foreach (Player player in players)
        {
            player.Broadcast(10, "\n<b>You're the <color=#ff4545>last one</color> on your team, don't disappoint others.</b>");
        }
    }

    public void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
    {
        if (ev.IsAllowed && ev.KnobSetting is Scp914KnobSetting.VeryFine)
        {
            if (Random.Range(0, 101) < 20)
            {
                ev.OutputPosition = RoleType.Scp93989.GetRandomSpawnProperties().Item1;
            }
        }
    }
    
    public void OnWaitingForPlayers()
    {
        CoroutinesHandler.Coroutines.Add(Timing.RunCoroutine(CoroutinesHandler.BetterDisarm()));
        CoroutinesHandler.Coroutines.Add(Timing.RunCoroutine(CoroutinesHandler.CleanerCoroutine()));
    }
}