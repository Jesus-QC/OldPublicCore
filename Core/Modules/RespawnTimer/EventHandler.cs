using System.Collections.Generic;
using System.Text;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;
using Random = UnityEngine.Random;

namespace Core.Modules.RespawnTimer
{
    public class EventHandler
    {
        private CoroutineHandle _timerCoroutine;
        public static List<string> Tips = new();

        public void OnRoundStarted()
        {
            _timerCoroutine = Timing.RunCoroutine(Timer());
        }

        public void OnEndedRound(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines(_timerCoroutine);
        }

        private static IEnumerator<float> Timer()
        {
            int i = 0;
            var tip = "This is a secret message, wow.";
            for (;;)
            {
                var builder = new StringBuilder("\nY<lowercase>ou will respawn in:</lowercase>\n");
                
                if (i == 16)
                {
                    i = 0;
                    tip = Tips[Random.Range(0, Tips.Count)];
                }
                
                yield return Timing.WaitForSeconds(0.95f);
                
                var seconds = Respawn.TimeUntilRespawn + (Respawn.IsSpawning ? 0 : 18);
                var minutes = seconds / 60;
                if (minutes != 0)
                    builder.Append(minutes + " minutes ");
                builder.Append(seconds % 60 + " seconds");

                if (Respawn.NextKnownTeam != SpawnableTeamType.None)
                {
                    builder.Append("\n\nAs a ");
                    if (Respawn.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
                        builder.Append("<color=#18f240>CHAOS</color>");
                    else
                        builder.Append("<color=#2542e6>M.T.F.</color>");
                }

                foreach (var player in Player.Get(Team.RIP))
                {
                    player.SendHint(ScreenZone.CenterBottom, builder.ToString(), 1.2f);
                    player.SendHint(ScreenZone.Bottom, "<color=#ab5ae8><u>TIP <sprite=12></u></color>\n<b><size=30>" + tip, 1.2f);
                }

                i++;
            }
        }
    }
}