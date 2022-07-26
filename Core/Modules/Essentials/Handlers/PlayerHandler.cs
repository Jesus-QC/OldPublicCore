using System.Linq;
using Core.Modules.Essentials.Extensions;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using InventorySystem.Items;
using MEC;

namespace Core.Modules.Essentials.Handlers
{
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

        public void OnWaitingForPlayers()
        {
           /* UnitNamingManager.RolesWithEnforcedDefaultName.Add(RoleType.ClassD, SpawnableTeamType.ChaosInsurgency);
            UnitNamingManager.RolesWithEnforcedDefaultName.Add(RoleType.Scientist, SpawnableTeamType.ChaosInsurgency);
            UnitNamingManager.RolesWithEnforcedDefaultName.Add(RoleType.ChaosConscript, SpawnableTeamType.ChaosInsurgency);
            UnitNamingManager.RolesWithEnforcedDefaultName.Add(RoleType.ChaosMarauder, SpawnableTeamType.ChaosInsurgency);
            UnitNamingManager.RolesWithEnforcedDefaultName.Add(RoleType.ChaosRepressor, SpawnableTeamType.ChaosInsurgency);
            UnitNamingManager.RolesWithEnforcedDefaultName.Add(RoleType.ChaosRifleman, SpawnableTeamType.ChaosInsurgency);
            UnitNamingManager.RolesWithEnforcedDefaultName.Add(RoleType.Spectator, SpawnableTeamType.ChaosInsurgency);

            string unit = RespawnManager.Singleton.NamingManager.AllUnitNames[0].UnitName;
            
            RespawnManager.Singleton.NamingManager.AllUnitNames.Remove(RespawnManager.Singleton.NamingManager.AllUnitNames[0]);

            RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit() { SpawnableTeam = (byte)SpawnableTeamType.ChaosInsurgency, UnitName = "<color=#d738ff><size=90%>discord.oblivionscp.xyz</size></color>" });
            RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit() { SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox, UnitName = "<color=#d738ff><size=90%>discord.oblivionscp.xyz</size></color>" });

            RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit() { SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox, UnitName = unit });
*/
            Timing.CallDelayed(0.5f, () => CoroutinesHandler.PickupAi = ItemSerialGenerator._ai);
            
            CoroutinesHandler.Coroutines.Add(Timing.RunCoroutine(CoroutinesHandler.BetterDisarm()));
            CoroutinesHandler.Coroutines.Add(Timing.RunCoroutine(CoroutinesHandler.CleanerCoroutine()));
        }
    }
}