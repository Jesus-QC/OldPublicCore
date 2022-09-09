using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using InventorySystem.Items.MicroHID;
using MEC;
using UnityEngine;

namespace Core.Modules.Levels.Events;

public class PlayerHandler
{
    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Player.Verified += OnVerified;
        Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.Dying += OnDying;
        Exiled.Events.Handlers.Player.Escaping += OnEscaping;
        Exiled.Events.Handlers.Player.RemovingHandcuffs += OnRemovingHandcuffs;
        Exiled.Events.Handlers.Scp914.UpgradingPlayer += OnUpgrading;
        Exiled.Events.Handlers.Player.ActivatingWorkstation += OnActivatingWorkstation;
        Exiled.Events.Handlers.Player.IntercomSpeaking += OnIntercom;
        Exiled.Events.Handlers.Player.EscapingPocketDimension += OnEscapingPocketDimension;
        Exiled.Events.Handlers.Player.ThrowingItem += OnThrowingItem;
        Exiled.Events.Handlers.Warhead.Starting += OnStartingWarhead;
        Exiled.Events.Handlers.Player.CancellingItemUse += OnCancellingItemUse;
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        Exiled.Events.Handlers.Player.ChangingMicroHIDState += OnChangingMicroStatus;
        Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
        Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
    }
    
    public void UnRegisterEvents()
    {
        Exiled.Events.Handlers.Player.Verified -= OnVerified;
        Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
        Exiled.Events.Handlers.Player.RemovingHandcuffs -= OnRemovingHandcuffs;
        Exiled.Events.Handlers.Scp914.UpgradingPlayer -= OnUpgrading;
        Exiled.Events.Handlers.Player.ActivatingWorkstation -= OnActivatingWorkstation;
        Exiled.Events.Handlers.Player.IntercomSpeaking -= OnIntercom;
        Exiled.Events.Handlers.Player.EscapingPocketDimension -= OnEscapingPocketDimension;
        Exiled.Events.Handlers.Player.ThrowingItem -= OnThrowingItem;
        Exiled.Events.Handlers.Warhead.Starting -= OnStartingWarhead;
        Exiled.Events.Handlers.Player.CancellingItemUse -= OnCancellingItemUse;
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
        Exiled.Events.Handlers.Player.ChangingMicroHIDState -= OnChangingMicroStatus;
        Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
        Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
    }

    private void OnVerified(VerifiedEventArgs ev)
    {
        LevelManager.Coroutines.Add(Timing.RunCoroutine(LevelManager.Explorer(ev.Player)));
    }

    private void OnUsedItem(UsedItemEventArgs ev)
    {
        switch (ev.Item.Type)
        {
            case ItemType.Medkit:
                ev.Player.AddUse(LevelToken.Restore);
                
                switch (ev.Player.GetUses(LevelToken.Restore))
                {
                    case 1:
                        ev.Player.AddExp(LevelToken.Curator, 25);
                        break;
                    case 3:
                        ev.Player.AddExp(LevelToken.Restore, 75);
                        break;
                }
                break;
            case ItemType.Adrenaline when ev.Player.CheckCooldown(LevelToken.Hyper,1):
                ev.Player.AddExp(LevelToken.Hyper, 30);
                break;
            case ItemType.Painkillers when ev.Player.CheckCooldown(LevelToken.Ointment, 1):
                ev.Player.AddExp(LevelToken.Ointment, 30);
                break;
        }

        if (!ev.Item.IsScp)
            return;
            
        ev.Player.AddUse(LevelToken.Deranged);

        switch (ev.Player.GetUses(LevelToken.Deranged))
        {
            case 1:
                ev.Player.AddExp(LevelToken.Mad, 50);
                break;
            case 2:
                ev.Player.AddExp(LevelToken.Psychotic, 100);
                break;
            case 3:
                ev.Player.AddExp(LevelToken.Deranged, 150);
                break;
        }

    }

    private void OnPickingUpItem(PickingUpItemEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Collect, 5))
            ev.Player.AddExp(LevelToken.Collect, 20);
        
        if (ev.Pickup.Type is ItemType.Coin)
        {
            switch (ev.Player.CountItem(ItemType.Coin))
            {
                case 2 when ev.Player.CheckCooldown(LevelToken.Cash, 1):
                    ev.Player.AddExp(LevelToken.Cash, 20);
                    break;
                case 8 when ev.Player.CheckCooldown(LevelToken.Ace, 1):
                    ev.Player.AddExp(LevelToken.Ace, 75);
                    break;
            }
        }
        else if(ev.Pickup.Type is ItemType.ParticleDisruptor && ev.Player.CheckCooldown(LevelToken.Particles, 1))
            ev.Player.AddExp(LevelToken.Particles, 50);
        else if(ev.Pickup.Type.IsWeapon() && ev.Player.CheckCooldown(LevelToken.Warlord, 1))
            ev.Player.AddExp(LevelToken.Warlord, 20);
        else if(ev.Pickup.Type.IsKeycard() && ev.Player.CheckCooldown(LevelToken.Access, 2))
            ev.Player.AddExp(LevelToken.Access, 20);
    }

    private void OnDying(DyingEventArgs ev)
    {
        if(ev.Killer is null || ev.Target is null || ev.Target == ev.Killer)
            return;
        
        if(ev.Killer.IsScp || ev.Target.IsScp)
            return;
        
        if(ev.Killer.CheckCooldown(LevelToken.Sharpshooter, 1) && Vector3.Distance(ev.Killer.Position, ev.Target.Position) > 10)
            ev.Killer.AddExp(LevelToken.Sharpshooter, 50);
    }
    
    private void OnDied(DiedEventArgs ev)
    {
        if(ev.Target is null)
            return;
            
        if(ev.Handler.Type is DamageType.Warhead && ev.Target.CheckCooldown(LevelToken.Detonate, 1))
            ev.Target.AddExp(LevelToken.Detonate, 20);
        else if(ev.Handler.Type is DamageType.Falldown && ev.Target.CheckCooldown(LevelToken.Deplete, 1))
            ev.Target.AddExp(LevelToken.Deplete, 50);
        else if(ev.Handler.Type is DamageType.Tesla && ev.Target.CheckCooldown(LevelToken.Electrified, 1))
            ev.Target.AddExp(LevelToken.Electrified, 20);
            
        if(ev.Killer is null || ev.Target == ev.Killer)
            return;

        if (ev.TargetOldRole.GetSide() is Side.Scp)
        {
            if (ev.TargetOldRole is RoleType.Scp0492)
            {
                if(ev.Killer.CheckCooldown(LevelToken.ZombieSlayer, 5))
                    ev.Killer.AddExp(LevelToken.ZombieSlayer, 25);
            }
            else
            {
                if (ev.Killer.CheckCooldown(LevelToken.MonsterHunter, 3))
                    ev.Killer.AddExp(LevelToken.MonsterHunter, 100);
            }
        }

        if (ev.Killer.Role.Side is Side.Scp)
        {
            switch (ev.Killer.Role.Type)
            {
                case RoleType.Scp106:
                    ev.Killer.AddExp(LevelToken.Decay, 30);
                    break;
                case RoleType.Scp173:
                    ev.Killer.AddExp(LevelToken.Snap, 20);
                    break;
                case RoleType.Scp93989 or RoleType.Scp93953:
                    ev.Killer.AddExp(LevelToken.Bite, 25);
                    break;
                case RoleType.Scp049:
                    ev.Killer.AddExp(LevelToken.Purge, 25);
                    break;
                case RoleType.Scp096:
                    ev.Killer.AddExp(LevelToken.Scream, 20);
                    break;
            }
        }
        else if (ev.Killer.Role.Team != Team.SCP && ev.TargetOldRole.GetTeam() != Team.SCP)
        {
            ev.Killer.AddExp(LevelToken.Erase, 15);
                
            if (ev.Killer.GetUses(LevelToken.Erase) == 10)
                ev.Killer.AddExp(LevelToken.SerialKiller, 100);
        }
            
        if(LevelManager.FirstKill)
            return;
            
        ev.Killer.AddExp(LevelToken.Hate, 30);
        LevelManager.FirstKill = true;
    }

    private void OnEscaping(EscapingEventArgs ev)
    {
        if(ev.Player.Role.Type == RoleType.ClassD && ev.Player.CheckCooldown(LevelToken.Disappear, 1))
            ev.Player.AddExp(LevelToken.Disappear, 250);
        else if(ev.Player.Role.Type == RoleType.Scientist && ev.Player.CheckCooldown(LevelToken.Renounce, 1))
            ev.Player.AddExp(LevelToken.Renounce, 200);
            
        if(Round.ElapsedTime.TotalMinutes < 5 && ev.Player.CheckCooldown(LevelToken.Jesus, 1))
            ev.Player.AddExp(LevelToken.Jesus, 300);
    }

    private void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
    {
        if(ev.Cuffer.CheckCooldown(LevelToken.Subdue, 3))
            ev.Cuffer.AddExp(LevelToken.Subdue, 30);
    }

    private void OnUpgrading(UpgradingPlayerEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Increase, 3))
            ev.Player.AddExp(LevelToken.Increase, 10);
    }

    private void OnActivatingWorkstation(ActivatingWorkstationEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Rebuild, 3))
            ev.Player.AddExp(LevelToken.Rebuild, 15);
    }

    private void OnIntercom(IntercomSpeakingEventArgs ev)
    {
        if (ev.Player is null || LevelManager.IntercomUsedPlayers.Contains(ev.Player))
            return;

        LevelManager.IntercomUsedPlayers.Add(ev.Player);
        ev.Player.AddExp(LevelToken.Gossip, 30);
    }

    private void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
    {
        if(ev.IsAllowed && ev.Player.CheckCooldown(LevelToken.NoMansLand, 1))
            ev.Player.AddExp(LevelToken.NoMansLand, 100);
    }

    private void OnThrowingItem(ThrowingItemEventArgs ev)
    {
        if(ev.Item.Type is ItemType.GrenadeFlash && ev.Player.CheckCooldown(LevelToken.Glimmer, 1))
            ev.Player.AddExp(LevelToken.Glimmer, 20);
    }

    private void OnStartingWarhead(StartingEventArgs ev)
    {
        if(ev.IsAuto || ev.Player is null || !ev.Player.CheckCooldown(LevelToken.Atomic, 1))
            return;
            
        ev.Player.AddExp(LevelToken.Atomic, 50);
    }

    private void OnCancellingItemUse(CancellingItemUseEventArgs ev)
    {
        switch (ev.Item.Type)
        {
            case ItemType.Medkit when ev.Player.CheckCooldown(LevelToken.Revoke, 1):
                ev.Player.AddExp(LevelToken.Revoke, 25);
                break;
        }
    }

    private void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if (!ev.IsAllowed || ev.Player is null)
            return;

        if(!LevelManager.DoorsDictionary.ContainsKey(ev.Player))
            LevelManager.DoorsDictionary.Add(ev.Player, new HashSet<Door>());

        HashSet<Door> dic = LevelManager.DoorsDictionary[ev.Player];

        if (!dic.Contains(ev.Door))
        {
            dic.Add(ev.Door);
            ev.Player.AddExp(LevelToken.Traveler, 5);
        }

        if (!ev.Door.IsGate || !ev.Player.CheckCooldown(LevelToken.Port, 3))
            return;
        
        ev.Player.AddExp(LevelToken.Port, 20);
    }

    private void OnChangingMicroStatus(ChangingMicroHIDStateEventArgs ev)
    {
        if(ev.NewState is HidState.Firing && ev.Player.CheckCooldown(LevelToken.Rupture, 1))
            ev.Player.AddExp(LevelToken.Rupture, 50);
    }

    private void OnDroppingItem(DroppingItemEventArgs ev)
    {
        switch (ev.IsThrown)
        {
            case true when ev.Player.CheckCooldown(LevelToken.Toss, 3):
            {
                ev.Player.AddExp(LevelToken.Toss, 10);
                break;
            }
            case false when ev.Player.CheckCooldown(LevelToken.Oops, 3):
            {
                ev.Player.AddExp(LevelToken.Oops, 20);
                break;
            }
        }
    }

    private void OnFlippingCoin(FlippingCoinEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Bet, 1))
            ev.Player.AddExp(LevelToken.Bet, 10);
    }
}