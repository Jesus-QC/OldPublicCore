using System.Linq;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Respawning;

namespace Core.Modules.Levels.Events
{
    public class ServerHandler
    {
        public void OnRestartingRound()
        {
            LevelManager.ClearCoroutines();
            LevelManager.FirstKill = false;
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (ev.LeadingTeam == LeadingTeam.ChaosInsurgency)
            {
                foreach (var ply in Player.List.Where(x => x.Role.Team == Team.CHI || x.Role == RoleType.ClassD))
                    ply.AddExp(50, Perk.Champions);
            }

            else if (ev.LeadingTeam == LeadingTeam.FacilityForces)
            {
                foreach (var ply in Player.List.Where(x => x.Role.Team == Team.MTF || x.Role == RoleType.Scientist))
                    ply.AddExp(30, Perk.Closed_Zone);
            }

            else if (ev.LeadingTeam == LeadingTeam.Anomalies)
            {
                foreach (var ply in Player.Get(Team.SCP))
                    ply.AddExp(25, Perk.Freedom);
            }
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
            {
                foreach (var player in Player.Get(RoleType.ClassD))
                {
                    if(player.CheckCooldown(Perk.We_Bring_Chaos, 1))
                        player.AddExp(20, Perk.We_Bring_Chaos);
                }
            }

            if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
            {
                foreach (var ply in Player.List.Where(x => x.Role == RoleType.Scientist || x.Role == RoleType.FacilityGuard))
                {
                    if(ply.CheckCooldown(Perk.Secure_Contain_Protect, 1))
                        ply.AddExp(20, Perk.Secure_Contain_Protect);
                }
            }
        }

        public void OnStartingWarhead(StartingEventArgs ev)
        {
            if(ev.Player.CheckCooldown(Perk.The_End, 1))
                ev.Player.AddExp(50, Perk.The_End);
        }
    }
}