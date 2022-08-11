using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Respawning;

namespace Core.Modules.Levels.Events;

public class ServerHandler
{
    public void RegisterEvents()
    {
        Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
    }
    
    public void UnRegisterEvents()
    {
        Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
    }
        
    public void OnRestartingRound()
    {
        LevelManager.ClearCoroutines();
        LevelManager.FirstKill = false; 
        LevelManager.IntercomUsedPlayers.Clear();
        LevelManager.DoorsDictionary.Clear();
    }

    public void OnRoundEnded(RoundEndedEventArgs ev)
    {
        if (ev.LeadingTeam is LeadingTeam.ChaosInsurgency)
            foreach (var ply in Player.Get(Side.ChaosInsurgency))
                ply.AddExp(LevelToken.Control);
        if(ev.LeadingTeam is LeadingTeam.Anomalies)
            foreach (var ply in Player.Get(Team.SCP))
                ply.AddExp(LevelToken.Destruction);

        foreach (var player in Player.List)
        {
            if(player.GetRoundExp() > 4999)
                player.AddExp(LevelToken.Cursed);
        }
    }

    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        switch (ev.NextKnownTeam)
        {
            case SpawnableTeamType.ChaosInsurgency:
            {
                foreach (var player in ev.Players)
                    if(player.CheckCooldown(LevelToken.Invaders, 1))
                        player.AddExp(LevelToken.Invaders);
                break;
            }
            case SpawnableTeamType.NineTailedFox:
            {
                foreach (var player in ev.Players)
                    if(player.CheckCooldown(LevelToken.Saviors, 1))
                        player.AddExp(LevelToken.Saviors);
                break;
            }
        }
    }
}