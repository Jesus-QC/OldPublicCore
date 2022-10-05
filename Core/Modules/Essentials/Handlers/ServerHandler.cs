using Core.Features.Components;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;
using UnityEngine;

namespace Core.Modules.Essentials.Handlers;

public class ServerHandler
{
    public void OnRestartingRound()
    {
        foreach (CoroutineHandle c in CoroutinesHandler.Coroutines)
            Timing.KillCoroutines(c);
            
        CoroutinesHandler.Coroutines.Clear();

        Server.FriendlyFire = false;
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

    public void OnRoundEnded(RoundEndedEventArgs ev) => Server.FriendlyFire = true;
    public void OnRoundStarted()
    {
        GameObject gameObject = new GameObject("SCP-106 Fix");
        gameObject.transform.position = Vector3.down * 2005;
        gameObject.transform.localScale = new Vector3(100,1,100);
        gameObject.AddComponent<JumpTrigger>();
    }
}