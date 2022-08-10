using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Features.Data.Configs;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Loader.Features;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using NorthwoodLib.Pools;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.SpectatorCount;

public class SpectatorCountModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "SpectatorCount";

    public override void OnEnabled()
    {
        DisabledManager.Load();
        
        Server.RoundEnded += OnEndedRound;
        Server.RoundStarted += OnRoundStarted;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Server.RoundEnded -= OnEndedRound;
        Server.RoundStarted -= OnRoundStarted;

        base.OnDisabled();
    }
    
    private Task _timerCoroutine;
    private CancellationTokenSource _cancellation;
    
    private void OnEndedRound(RoundEndedEventArgs ev)
    {
        _cancellation.Cancel();
    }

    private void OnRoundStarted()
    {
        if (_cancellation is not null)
        {
            _cancellation.Dispose();
        }
        
        _cancellation = new CancellationTokenSource();
        _timerCoroutine = Task.Run(Timer, _cancellation.Token);
    }

    private async Task Timer()
    {
        var builder = StringBuilderPool.Shared.Rent();
        while (true)
        {
            if (_cancellation.IsCancellationRequested)
            {
                StringBuilderPool.Shared.Return(builder);
                return;
            }
            
            foreach (var player in Player.List)
            {
                if(player is null || player.IsDead || DisabledManager.IsHidden(player))
                    continue;

                builder.Clear();
                builder.Append("<align=right><size=75%><color=#555><b>👥 Spectators:</b></color><color=" + player.Role.Color.ToHex() + ">");

                int count = 0;
                foreach (var spectator in player.CurrentSpectatingPlayers)
                {
                    if(spectator is null || spectator == player || spectator.IsGlobalModerator || spectator.IsOverwatchEnabled)
                        continue;
                    
                    count++;
                    
                    if(count == 5)
                    {
                        builder.Append($"\n</color><color=#555><b>{player.CurrentSpectatingPlayers.Count() - 4} more</b>");
                        break;
                    }

                    builder.Append("\n" + spectator.Nickname);
                }

                if (count == 0)
                    continue;

                builder.Append("</color></size></align>");
                
                player.SendHint(ScreenZone.Top, builder.ToString(), 1);
            }

            await Task.Delay(1000);
        }
    }
}