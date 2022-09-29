using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Features.Logger;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using NorthwoodLib.Pools;
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
        _cancellation?.Dispose();

        _cancellation = new CancellationTokenSource();
        Task.Run(Timer, _cancellation.Token);
    }

    public static string Tip = string.Empty;
    public static string RenderedZone = string.Empty;
    
    private async Task Timer()
    {
        Log.Info($"{LogUtils.GetColor(LogColor.Yellow)}Started RespawnTimer Timer");
        
        int i = 0;
        StringBuilder builder = StringBuilderPool.Shared.Rent();
        for (;;)
        {
            if (_cancellation.IsCancellationRequested)
            {
                StringBuilderPool.Shared.Return(builder);
                return;
            }

            builder.Clear();
            
            if (Respawn.NextKnownTeam != SpawnableTeamType.None)
            {
                builder.Append("a ");
                builder.Append(Respawn.NextKnownTeam == SpawnableTeamType.ChaosInsurgency
                    ? "<color=#18f240>chaos</color>"
                    : "<color=#2542e6>m.t.f.</color>");
            }

            builder.AppendLine();
            builder.AppendLine(Respawn.IsSpawning ? "W<lowercase>ave will spawn in:</lowercase>" : "N<lowercase>ext team is on the way!</lowercase>");

            if (i == 25)
            {
                i = 0;
                Tip = "<color=#9342f5>❓</color>" + Tips[Random.Range(0, Tips.Count)];
            }
            
            await Task.Delay(1000);
                
            if (Respawn.TimeUntilSpawnWave.Minutes != 0)
                builder.Append(Respawn.TimeUntilSpawnWave.Minutes + " minutes ");
            builder.AppendLine(Respawn.TimeUntilSpawnWave.Seconds + " seconds\n");

            builder.Append("\n" + GetCount());

            RenderedZone = builder.ToString();

            i++;
        }
    }

    private static string GetCount()
    {
        return $"<size=90%><color=#9effe0>👻 spectators:</color> {Player.List.Count(x => x.Role.Type == RoleType.Spectator)} | <color=#9ecfff>▣ mtf tickets:</color> {Respawn.NtfTickets} | <color=#9effa6>⛔ chaos tickets:</color> {Respawn.ChaosTickets}</size>";
    }
}