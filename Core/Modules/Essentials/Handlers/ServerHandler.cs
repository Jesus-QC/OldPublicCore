using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;

namespace Core.Modules.Essentials.Handlers;

public class ServerHandler
{
    private int _rounds;
        
    public void OnRestartingRound()
    {
        foreach (CoroutineHandle c in CoroutinesHandler.Coroutines)
            Timing.KillCoroutines(c);
            
        CoroutinesHandler.Coroutines.Clear();
            
        _rounds++;
            
        if (_rounds == EssentialsModule.PluginConfig.RoundsToRestart)
            Server.Restart();
    }

    public void OnAnnouncingMtfEntrance(AnnouncingNtfEntranceEventArgs ev)
    {
        ev.IsAllowed = false;
        Cassie.MessageTranslated(EssentialsModule.PluginConfig.MtfAnnouncement.Replace("%unit%", ev.UnitName).Replace("%unitnumber%", ev.UnitNumber.ToString()).Replace("%scps%", ev.ScpsLeft.ToString()),
            $"Mtf Unit <color=blue>{ev.UnitName} {ev.UnitNumber}</color> Designated NineTailedFox has entered the facility, awaiting recontainment of <color=red>{ev.ScpsLeft} SCPs.</color>");
    }

    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (ev.NextKnownTeam != SpawnableTeamType.ChaosInsurgency)
            return;
                
        Cassie.MessageTranslated(EssentialsModule.PluginConfig.ChaosAnnouncement,
            "Emergency Alert, Unauthorized Military Group... Scaning threat... Threat designated as <color=green>Chaos Insurgency</color>");
    }
}