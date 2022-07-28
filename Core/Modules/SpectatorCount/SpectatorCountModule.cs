using System.Collections.Generic;
using System.Linq;
using Core.Features.Data.Configs;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Loader.Features;
using Exiled.API.Features;
using MEC;
using NorthwoodLib.Pools;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.SpectatorCount;

public class SpectatorCountModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "SpectatorCount";

    public override void OnEnabled()
    {
        DisabledManager = new DisabledManager();
        DisabledManager.Load();
        
        Server.RestartingRound += OnRestartingRound;
        Server.RoundStarted += OnStartingRound;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Server.RestartingRound -= OnRestartingRound;
        Server.RoundStarted -= OnStartingRound;

        DisabledManager = null;
        
        base.OnDisabled();
    }

    private CoroutineHandle _coroutine;
    public static DisabledManager DisabledManager;

    private void OnRestartingRound()
    {
        if (_coroutine.IsRunning)
            Timing.KillCoroutines(_coroutine);
    }

    private void OnStartingRound()
    {
        _coroutine = Timing.RunCoroutine(SpectatorList());
    }

    private IEnumerator<float> SpectatorList()
    {
        while (true)
        {
            foreach (var player in Player.List)
            {
                if(player is null || player.IsDead || DisabledManager.IsHidden(player))
                    continue;

                var builder = StringBuilderPool.Shared.Rent();
                builder.Append("<align=right><size=75%><color=#555><b>👥 Spectators:</b></color><color=" + player.Role.Color.ToHex() + ">");

                int count = 0;
                foreach (var spectator in player.CurrentSpectatingPlayers)
                {
                    if(spectator is null || spectator == player || spectator.IsGlobalModerator || spectator.IsOverwatchEnabled)
                        continue;
                    
                    count++;
                    
                    if(count == 5)
                    {
                        builder.Append($"\n</color><color={player.Role.Color.ToHex()}>{player.CurrentSpectatingPlayers.Count() - 4} more");
                        break;
                    }

                    builder.Append("\n" + spectator.Nickname);
                }

                if (count == 0)
                {
                    StringBuilderPool.Shared.Return(builder);
                    continue;
                }

                builder.Append("</color></size></align>");
                
                player.SendHint(ScreenZone.Top, StringBuilderPool.Shared.ToStringReturn(builder), 1);
            }

            yield return Timing.WaitForSeconds(0.95f);
        }
    }
}