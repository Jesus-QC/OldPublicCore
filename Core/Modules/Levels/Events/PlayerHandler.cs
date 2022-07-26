using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects;
using InventorySystem.Items.MicroHID;
using MEC;

namespace Core.Modules.Levels.Events
{
    public class PlayerHandler
    {
        public void OnVerified(VerifiedEventArgs ev)
        {
            LevelManager.Coroutines.Add(Timing.RunCoroutine(LevelManager.Explorer(ev.Player)));
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if(ev.Pickup.Type == ItemType.KeycardO5 && ev.Player.CheckCooldown(Perk.Council_Approved, 1))
                ev.Player.AddExp(30, Perk.Council_Approved);
            else if(ev.Pickup.Type == ItemType.Coin && ev.Player.CountItem(ItemType.Coin) == 2 && ev.Player.CheckCooldown(Perk.Pocket_Money, 1))
                ev.Player.AddExp(20, Perk.Pocket_Money);
            else if(ev.Pickup.Type.IsWeapon() && ev.Player.CheckCooldown(Perk.Rambo, 1))
                ev.Player.AddExp(20, Perk.Rambo);

            if (ev.Player.CheckCooldown(Perk.Gather, 5)) 
                ev.Player.AddExp(5, Perk.Gather);
        }

        public void OnCancellingItemUse(CancellingItemUseEventArgs ev)
        {
            if(ev.Player.CheckCooldown(Perk.Choices, 1))
                ev.Player.AddExp(10, Perk.Choices);
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Door.Base is PryableDoor && ev.Player.CheckCooldown(Perk.Gandalf, 1))
                ev.Player.AddExp(20, Perk.Gandalf);
            
        }

        public void OnEscaping(EscapingEventArgs ev)
        {
            if(ev.Player.IsCuffed && ev.Player.CheckCooldown(Perk.Strain, 1))
                ev.Player.AddExp(100, Perk.Strain);
            if(ev.Player.Role == RoleType.ClassD && ev.Player.CheckCooldown(Perk.Vanish, 1))
                ev.Player.AddExp(200, Perk.Vanish);
            if(ev.Player.Role == RoleType.Scientist && ev.Player.CheckCooldown(Perk.Depart, 1))
                ev.Player.AddExp(150, Perk.Depart);
            
            if(Round.ElapsedTime.TotalMinutes > 4 && ev.Player.CheckCooldown(Perk.Jesus, 1))
                ev.Player.AddExp(300, Perk.Jesus);
        }

        public void OnEscapingPocket(EscapingPocketDimensionEventArgs ev)
        {
            if(ev.Player.CheckCooldown(Perk.Maze_Runner, 2))
                ev.Player.AddExp(25, Perk.Maze_Runner);
        }

        public void OnDying(DyingEventArgs ev)
        {
            if(ev.Killer == null || ev.Target == null) return;

            if (ev.Target.Role == RoleType.Scp106)
            {
                foreach (var ply in Player.List)
                    if(ply.SessionVariables.ContainsKey("t106") && ply.CheckCooldown(Perk.Sink_Back, 1))
                        ply.AddExp(100, Perk.Sink_Back);
            }
            else if (ev.Target.Role == RoleType.Scp096)
            {
                foreach (var ply in Player.List)
                    if(ply.SessionVariables.ContainsKey("t096") && ply.CheckCooldown(Perk.Silence, 1))
                        ply.AddExp(100, Perk.Silence);
            }
            
            if(ev.Killer == ev.Target) return;

            if(ev.Target.IsScp && ev.Killer.CheckCooldown(Perk.Last_Stand, 1))
                ev.Killer.AddExp(50, Perk.Last_Stand);
            if(ev.Target.Role == RoleType.Scp0492 && ev.Killer.CheckCooldown(Perk.Really_Cured, 5))
                ev.Killer.AddExp(20, Perk.Really_Cured);

            if (ev.Target.Role.Team == Team.SCP)
            {
                var count = ev.Killer.GetUses(Perk.SCP_Hunter) + 1;
                
                if(count == 3)
                    ev.Killer.AddExp(100, Perk.SCP_Hunter);
                else if(count == 5)
                    ev.Killer.AddExp(200, Perk.SCP_Destroyer);
                else
                    ev.Killer.AddUse(Perk.SCP_Hunter);
            }
            
            if (!ev.Killer.IsHuman || !ev.Target.IsHuman) return;

            var uses = ev.Killer.GetUses(Perk.Slay);
            if (uses < 5)
                ev.Killer.AddExp(uses * 5 + 10);
        }

        public void OnDied(DiedEventArgs ev)
        {
            if(ev.Target == null) return;
            
            if(ev.Handler.Type == DamageType.Warhead && ev.Target.CheckCooldown(Perk.Blasted, 1))
                ev.Target.AddExp(30, Perk.Blasted);
            if(ev.Handler.Type == DamageType.Falldown && ev.Target.CheckCooldown(Perk.Broken_Bones, 1))
                ev.Target.AddExp(20, Perk.Broken_Bones);
            if(ev.Handler.Type == DamageType.Tesla && ev.Target.CheckCooldown(Perk.Fried, 3))
                ev.Target.AddExp(10, Perk.Fried);

            if(ev.Killer == ev.Target || ev.Killer == null) return;
            
            if (!LevelManager.FirstKill)
            {
                LevelManager.FirstKill = true;
                ev.Killer.AddExp(30, Perk.First_Blood);
            }

            var kills = ev.Killer.GetUses(Perk.Bloodthirst);
            if(kills < 4)
                ev.Killer.AddUse(Perk.Bloodthirst);
            else if (kills == 4)
                ev.Killer.AddExp(50, Perk.Bloodthirst);
            
            if(ev.Killer.Role.Team != Team.SCP) return;
            
            if (ev.Killer.Role == RoleType.Scp106)
                ev.Killer.AddExp(25, Perk.SCP_106);
            else if (ev.Killer.Role == RoleType.Scp173)
                ev.Killer.AddExp(20, Perk.SCP_173);
            else if (ev.Killer.Role == RoleType.Scp93953)
                ev.Killer.AddExp(25, Perk.SCP_93953);
            else if (ev.Killer.Role == RoleType.Scp93989)
                ev.Killer.AddExp(15, Perk.SCP_93989);
            else if (ev.Killer.Role == RoleType.Scp049)
                ev.Killer.AddExp(15, Perk.SCP_049);
            else if (ev.Killer.Role == RoleType.Scp096) 
                ev.Killer.AddExp(10, Perk.SCP_096);
        }

        public void OnChangingMicroHIDState(ChangingMicroHIDStateEventArgs ev)
        {
            if(ev.NewState == HidState.Firing && ev.Player.CheckCooldown(Perk.Overcharge, 1))
                ev.Player.AddExp(50, Perk.Overcharge);
        }

        public void OnUsedItem(UsedItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Medkit)
            {
                if(ev.Player.CheckCooldown(Perk.Stitching, 3))
                    ev.Player.AddExp(10, Perk.Stitching);

                var uses = ev.Player.GetUses(Perk.Immortal);
                if (uses < 3)
                    ev.Player.AddUse(Perk.Immortal);
                else if (uses == 3)
                    ev.Player.AddExp(30, Perk.Immortal);
            }
            else if(ev.Item.Type == ItemType.Adrenaline)
            {
                var uses = ev.Player.GetUses(Perk.Syringe);
                if(uses < 3)
                    ev.Player.AddUse(Perk.Syringe);
                else if (uses == 3)
                    ev.Player.AddExp(20, Perk.Syringe);
            }
            else if (ev.Item.Type.IsScp())
            {
                if(ev.Player.CheckCooldown(Perk.Intensify, 1))
                    ev.Player.AddExp(50, Perk.Intensify);

                var uses = ev.Player.GetUses(Perk.Cheater);
                if(uses < 2)
                    ev.Player.AddUse(Perk.Cheater);
                else if (uses == 2)
                    ev.Player.AddExp(75, Perk.Cheater);
            }
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if(ev.Attacker == null || ev.Target == null) return;

            if(ev.Attacker.Id == ev.Target.Id) return;

            if (ev.Target.Role == RoleType.Scp106 && !ev.Attacker.SessionVariables.ContainsKey("t106"))
                ev.Attacker.SessionVariables.Add("t106", true);
            else if (ev.Target.Role == RoleType.Scp096 && !ev.Attacker.SessionVariables.ContainsKey("t096"))
                ev.Attacker.SessionVariables.Add("t096", true);
        }

        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if(ev.Cuffer.CheckCooldown(Perk.Cripple, 2))
                ev.Cuffer.AddExp(30, Perk.Cripple);
        }

        public void OnActivatingWorkstation(ActivatingWorkstationEventArgs ev)
        {
            if(ev.Player.CheckCooldown(Perk.Adapt, 1))
                ev.Player.AddExp(20, Perk.Adapt);
        }

        public void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            if(ev.Player.CheckCooldown(Perk.Radio_Host, 1))
                ev.Player.AddExp(20, Perk.Radio_Host);
        }

        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if(ev.Player.CheckCooldown(Perk.Oops, 1))
                ev.Player.AddExp(10, Perk.Oops);
            if(ev.IsThrown && ev.Player.CheckCooldown(Perk.Toss, 1))
                ev.Player.AddExp(10, Perk.Toss);
        }

        public void OnThrowingItem(ThrowingItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.GrenadeHE)
            {
                var uses = ev.Player.GetUses(Perk.Bomber);
                if(uses < 2)
                    ev.Player.AddUse(Perk.Bomber);
                else if (uses == 2)
                    ev.Player.AddExp(25, Perk.Bomber);
            }
            
            if (ev.Item.Type == ItemType.GrenadeFlash)
            {
                var uses = ev.Player.GetUses(Perk.Lightning);
                if(uses < 2)
                    ev.Player.AddUse(Perk.Lightning);
                else if (uses == 2)
                    ev.Player.AddExp(25, Perk.Lightning);
            }
            
            if(ev.Item.Type == ItemType.GrenadeHE && ev.Player.CheckCooldown(Perk.Baseball, 1))
                ev.Player.AddExp(25, Perk.Baseball);
            else if(ev.Item.Type == ItemType.GrenadeFlash && ev.Player.CheckCooldown(Perk.Disappear, 1))
                ev.Player.AddExp(25, Perk.Disappear);
        }

        public void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if(ev.Player.CheckCooldown(Perk.Heads_Or_Tails, 3))
                ev.Player.AddExp(10, Perk.Heads_Or_Tails);
        }

        public void OnEnteringPocket(EnteringPocketDimensionEventArgs ev)
        {
            if(ev.Scp106.CheckCooldown(Perk.Dark_Pocket, 5))
                ev.Scp106.AddExp(10, Perk.Dark_Pocket);
        }

        public void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if(ev.UpgradeItems && ev.Player.CheckCooldown(Perk.Progress, 3))
                ev.Player.AddExp(15, Perk.Progress);
        }
    }
}