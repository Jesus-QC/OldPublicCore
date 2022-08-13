using System.Linq;
using Core.Modules.Essentials.Extensions;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using MEC;
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
            
        if (Random.Range(0,100) <= EssentialsModule.PluginConfig.FlashlightChance)
            ev.Items.Add(ItemType.Flashlight);
    }

    public void OnWaitingForPlayers()
    {
        CoroutinesHandler.Coroutines.Add(Timing.RunCoroutine(CoroutinesHandler.BetterDisarm()));
        CoroutinesHandler.Coroutines.Add(Timing.RunCoroutine(CoroutinesHandler.CleanerCoroutine()));
    }
}