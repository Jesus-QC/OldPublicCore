using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                        ev.Player.AddExp(LevelToken.Curator);
                        break;
                    case 3:
                        ev.Player.AddExp(LevelToken.Restore);
                        break;
                }
                break;
            case ItemType.Adrenaline when ev.Player.CheckCooldown(LevelToken.Hyper,1):
                ev.Player.AddExp(LevelToken.Hyper);
                break;
            case ItemType.Painkillers when ev.Player.CheckCooldown(LevelToken.Ointment, 1):
                ev.Player.AddExp(LevelToken.Ointment);
                break;
        }

        if (!ev.Item.IsScp)
            return;
            
        ev.Player.AddUse(LevelToken.Deranged);

        switch (ev.Player.GetUses(LevelToken.Deranged))
        {
            case 1:
                ev.Player.AddExp(LevelToken.Mad);
                break;
            case 2:
                ev.Player.AddExp(LevelToken.Psychotic);
                break;
            case 3:
                ev.Player.AddExp(LevelToken.Deranged);
                break;
        }

    }

    private void OnPickingUpItem(PickingUpItemEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Collect, 5))
            ev.Player.AddExp(LevelToken.Collect);
            
        if (ev.Pickup.Type is ItemType.Coin)
        {
            switch (ev.Player.CountItem(ItemType.Coin))
            {
                case 2 when ev.Player.CheckCooldown(LevelToken.Cash, 1):
                    ev.Player.AddExp(LevelToken.Cash);
                    break;
                case 8 when ev.Player.CheckCooldown(LevelToken.Ace, 1):
                    ev.Player.AddExp(LevelToken.Ace);
                    break;
            }
        }
        else if(ev.Pickup.Type.IsKeycard() && ev.Player.CheckCooldown(LevelToken.Access, 2))
            ev.Player.AddExp(LevelToken.Access);
        else if(ev.Pickup.Type.IsWeapon() && ev.Player.CheckCooldown(LevelToken.Warlord, 1))
            ev.Player.AddExp(LevelToken.Warlord);
    }
        
    private void OnDied(DiedEventArgs ev)
    {
        if(ev.Target is null)
            return;
            
        if(ev.Handler.Type is DamageType.Warhead && ev.Target.CheckCooldown(LevelToken.Detonate, 1))
            ev.Target.AddExp(LevelToken.Detonate);
        else if(ev.Handler.Type is DamageType.Falldown && ev.Target.CheckCooldown(LevelToken.Deplete, 1))
            ev.Target.AddExp(LevelToken.Deplete);
        else if(ev.Handler.Type is DamageType.Tesla && ev.Target.CheckCooldown(LevelToken.Electrified, 1))
            ev.Target.AddExp(LevelToken.Electrified);
            
        if(ev.Killer is null || ev.Target == ev.Killer)
            return;

        if (ev.TargetOldRole.GetSide() is Side.Scp)
        {

            if (ev.TargetOldRole is RoleType.Scp0492)
            {
                if(ev.Killer.CheckCooldown(LevelToken.ZombieSlayer, 5))
                    ev.Killer.AddExp(LevelToken.ZombieSlayer);
            }
            else
            {
                Task.Run(() =>
                {
                    Collider[] colliders = Array.Empty<Collider>();
                    Physics.OverlapSphereNonAlloc(ev.Killer.Position, 5f, colliders);
                    foreach (var go in colliders)
                    {
                        if (!Player.TryGet(go.gameObject, out var ply))
                            return;

                        if (ply.CheckCooldown(LevelToken.MonsterHunter, 3))
                            ply.AddExp(LevelToken.MonsterHunter);
                    }
                });
            }
        }

        if (ev.Killer.Role.Side is Side.Scp)
        {
            switch (ev.Killer.Role.Type)
            {
                case RoleType.Scp106:
                    ev.Killer.AddExp(LevelToken.Decay);
                    break;
                case RoleType.Scp173:
                    ev.Killer.AddExp(LevelToken.Snap);
                    break;
                case RoleType.Scp93989 or RoleType.Scp93953:
                    ev.Killer.AddExp(LevelToken.Bite);
                    break;
                case RoleType.Scp049:
                    ev.Killer.AddExp(LevelToken.Purge);
                    break;
                case RoleType.Scp096:
                    ev.Killer.AddExp(LevelToken.Scream);
                    break;
            }
        }
        else if (ev.Killer.Role.Team != Team.SCP && ev.TargetOldRole.GetTeam() != Team.SCP)
        {
            ev.Killer.AddExp(LevelToken.Erase);
                
            if(ev.Killer.GetUses(LevelToken.Erase) == 10)
                ev.Killer.AddExp(LevelToken.SerialKiller);
        }
            
        if(LevelManager.FirstKill)
            return;
            
        ev.Killer.AddExp(LevelToken.Hate);
        LevelManager.FirstKill = true;
    }

    private void OnEscaping(EscapingEventArgs ev)
    {
        if(ev.Player.Role.Type == RoleType.ClassD && ev.Player.CheckCooldown(LevelToken.Disappear, 1))
            ev.Player.AddExp(LevelToken.Disappear);
        else if(ev.Player.Role.Type == RoleType.Scientist && ev.Player.CheckCooldown(LevelToken.Renounce, 1))
            ev.Player.AddExp(LevelToken.Renounce);
            
        if(Round.ElapsedTime.TotalMinutes < 5 && ev.Player.CheckCooldown(LevelToken.Jesus, 1))
            ev.Player.AddExp(LevelToken.Jesus);
    }

    private void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
    {
        if(ev.Cuffer.CheckCooldown(LevelToken.Subdue, 3))
            ev.Cuffer.AddExp(LevelToken.Subdue);
    }

    private void OnUpgrading(UpgradingPlayerEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Increase, 3))
            ev.Player.AddExp(LevelToken.Increase);
    }

    private void OnActivatingWorkstation(ActivatingWorkstationEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Rebuild, 3))
            ev.Player.AddExp(LevelToken.Rebuild);
    }

    private void OnIntercom(IntercomSpeakingEventArgs ev)
    {
        if (ev.Player is null || LevelManager.IntercomUsedPlayers.Contains(ev.Player))
            return;

        LevelManager.IntercomUsedPlayers.Add(ev.Player);
        ev.Player.AddExp(LevelToken.Gossip);
    }

    private void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
    {
        if(ev.IsAllowed && ev.Player.CheckCooldown(LevelToken.NoMansLand, 1))
            ev.Player.AddExp(LevelToken.NoMansLand);
    }

    private void OnThrowingItem(ThrowingItemEventArgs ev)
    {
        if(ev.Item.Type is ItemType.GrenadeFlash && ev.Player.CheckCooldown(LevelToken.Glimmer, 1))
            ev.Player.AddExp(LevelToken.Glimmer);
    }

    private void OnStartingWarhead(StartingEventArgs ev)
    {
        if(ev.IsAuto || ev.Player is null || !ev.Player.CheckCooldown(LevelToken.Atomic, 1))
            return;
            
        ev.Player.AddExp(LevelToken.Atomic);
    }

    private void OnCancellingItemUse(CancellingItemUseEventArgs ev)
    {
        switch (ev.Item.Type)
        {
            case ItemType.Medkit when ev.Player.CheckCooldown(LevelToken.Revoke, 1):
                ev.Player.AddExp(LevelToken.Revoke);
                break;
        }
    }

    private void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if(!ev.IsAllowed)
            return;

        var ply = ev.Player;
            
        if(!LevelManager.DoorsDictionary.ContainsKey(ev.Player))
            LevelManager.DoorsDictionary.Add(ev.Player, new HashSet<Door>());

        var dic = LevelManager.DoorsDictionary[ev.Player];

        if (!dic.Contains(ev.Door))
        {
            ev.Player.AddExp(LevelToken.Traveler);
            dic.Add(ev.Door);
        }

        if (!ev.Door.IsGate || !ev.Player.CheckCooldown(LevelToken.Port, 3))
            return;
        
        ev.Player.AddExp(LevelToken.Port);
    }

    private void OnChangingMicroStatus(ChangingMicroHIDStateEventArgs ev)
    {
        if(ev.NewState is HidState.Firing && ev.Player.CheckCooldown(LevelToken.Rupture, 1))
            ev.Player.AddExp(LevelToken.Rupture);
    }

    private void OnDroppingItem(DroppingItemEventArgs ev)
    {
        switch (ev.IsThrown)
        {
            case true when ev.Player.CheckCooldown(LevelToken.Toss, 3):
            {
                ev.Player.AddExp(LevelToken.Toss);
                break;
            }
            case false when ev.Player.CheckCooldown(LevelToken.Oops, 3):
            {
                ev.Player.AddExp(LevelToken.Oops);
                break;
            }
        }
    }

    private void OnFlippingCoin(FlippingCoinEventArgs ev)
    {
        if(ev.Player.CheckCooldown(LevelToken.Bet, 1))
            ev.Player.AddExp(LevelToken.Bet);
    }
}