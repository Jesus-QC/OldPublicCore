using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Respawning;
using Random = UnityEngine.Random;

namespace Core.Modules.RespawnTimer;

public class EventHandler
{
    private CancellationTokenSource _cancellation;
    
    public static List<string> Tips = new();

    public void OnEndedRound(RoundEndedEventArgs ev)
    {
        _cancellation.Cancel();
    }

    public void OnRoundStarted()
    {
        if (_cancellation is not null)
        {
            _cancellation.Dispose();
        }
        
        _cancellation = new CancellationTokenSource();
        Task.Run(Timer, _cancellation.Token);
    }
    
    private async Task Timer()
    {
        int i = 0;
        var tip = "This is a secret message, wow.";
        var builder = new StringBuilder();
        var tipBuilder = new StringBuilder();
        for (;;)
        {
            if (_cancellation.IsCancellationRequested)
                return;

            builder.Clear();
            tipBuilder.Clear();

            builder.Append(Respawn.IsSpawning ? "\n\n\n\nN<lowercase>ext team will respawn in:</lowercase>\n" : "\n\n\n\nN<lowercase>ext team is on the way!</lowercase>\n");
            tipBuilder.AppendLine();
                
            if (i == 16)
            {
                i = 0;
                tip = Tips[Random.Range(0, Tips.Count)];
            }
            
            await Task.Delay(1000);
                
            if (Respawn.TimeUntilSpawnWave.Minutes != 0)
                builder.Append(Respawn.TimeUntilSpawnWave.Minutes + " minutes ");
            builder.Append(Respawn.TimeUntilSpawnWave.Seconds + " seconds");

                
            if (Respawn.NextKnownTeam != SpawnableTeamType.None)
            {
                tipBuilder.Append("as ");
                tipBuilder.Append(Respawn.NextKnownTeam == SpawnableTeamType.ChaosInsurgency
                    ? "<color=#18f240>chaos</color>"
                    : "<color=#2542e6>m.t.f.</color>");
            }

            tipBuilder.Append("\n\n" + GetCount() + "<size=70%><color=#9342f5>❓</color>" + tip + "</size>");

            var text = builder.ToString();
            var tipText = tipBuilder.ToString();
                
            foreach (var player in Player.List)
            {
                if(player.Role.Type != RoleType.Spectator)
                    continue;
                
                player.SendHint(ScreenZone.Center, text, 1.2f);
                player.SendHint(ScreenZone.Bottom, tipText, 1.2f);
            }

            i++;
        }
    }

    private static string GetCount()
    {
        return $"<color=#9effe0>👻 spectators:</color> {Player.List.Count(x => x.Role.Type == RoleType.Spectator)} | <color=#9ecfff>▣ mtf tickets:</color> {Respawn.NtfTickets} | <color=#9effa6>⛔ chaos tickets:</color> {Respawn.ChaosTickets}\n";
    }
}