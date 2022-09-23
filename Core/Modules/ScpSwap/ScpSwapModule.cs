using System.Collections.Generic;
using Core.Features.Data.Configs;
using Core.Loader.Features;
using Exiled.Events.Handlers;
using MEC;
using Player = Exiled.API.Features.Player;

namespace Core.Modules.ScpSwap;

public class ScpSwapModule : CoreModule<EmptyConfig>
{
    public override string Name => "ScpSwap";

    public override void OnEnabled()
    {
        Server.RoundStarted += OnRoundStarted;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Server.RoundStarted -= OnRoundStarted;
        
        base.OnDisabled();
    }

    private void OnRoundStarted()
    {
        ScpSwapCommand.IsOpened = true;
        Timing.CallDelayed(3, ShowScp);
        Timing.CallDelayed(30, () => ScpSwapCommand.IsOpened = false);
    }

    private void ShowScp()
    {
        HashSet<Player> scp = new();
        string msg = string.Empty;

        foreach (Player p in Player.Get(Team.SCP))
        {
            msg += $"| {p.Role.Type} | ";
            scp.Add(p);
        }

        foreach (Player scpP in scp)
            scpP.Broadcast(10, $@"\n<size=30><b>Don't you like your SCP?\nUse <color=#aaa>.scpswap</color> to swap it!\n<color=#EC2121><size=20>TEAMMATES:</size>\n{msg.TrimStart(' ').TrimEnd(' ')}</color></b></size>");
    }
}