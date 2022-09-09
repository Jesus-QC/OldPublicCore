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
            foreach (Player ply in Player.Get(Side.ChaosInsurgency))
                ply.AddExp(LevelToken.CrimesPay, 20);
        if(ev.LeadingTeam is LeadingTeam.Anomalies)
            foreach (Player ply in Player.Get(Team.SCP))
                ply.AddExp(LevelToken.Destruction, 20);

        foreach (Player player in Player.List)
        {
            if(player.GetRoundExp() > 4999)
                player.AddExp(LevelToken.Cursed, 250);
        }
    }

    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        switch (ev.NextKnownTeam)
        {
            case SpawnableTeamType.ChaosInsurgency:
            {
                foreach (Player player in ev.Players)
                    if(player.CheckCooldown(LevelToken.Invaders, 1))
                        player.AddExp(LevelToken.Invaders, 20);
                break;
            }
            case SpawnableTeamType.NineTailedFox:
            {
                foreach (Player player in ev.Players)
                    if(player.CheckCooldown(LevelToken.Saviors, 1))
                        player.AddExp(LevelToken.Saviors, 20);
                break;
            }
        }
    }
}