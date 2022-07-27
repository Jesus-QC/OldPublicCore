using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;

namespace Core.Modules.Essentials.Handlers
{
    public class ServerHandler
    {
        public void OnRestartingRound()
        {
            foreach (var c in CoroutinesHandler.Coroutines)
                Timing.KillCoroutines(c);
            
            CoroutinesHandler.Coroutines.Clear();
        }

        public void OnAnnouncingMtfEntrance(AnnouncingNtfEntranceEventArgs ev)
        {
            ev.IsAllowed = false;
            Cassie.Message(EssentialsModule.PluginConfig.MtfAnnouncement.Replace("%unit%", ev.UnitName).Replace("%unitnumber%", ev.UnitNumber.ToString()).Replace("%scps%", ev.ScpsLeft.ToString()));
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam != SpawnableTeamType.ChaosInsurgency)
                return;
                
            Cassie.Message(EssentialsModule.PluginConfig.ChaosAnnouncement.Replace("%scps%", Player.Get(Team.SCP).Count().ToString()));
        }
    }
}