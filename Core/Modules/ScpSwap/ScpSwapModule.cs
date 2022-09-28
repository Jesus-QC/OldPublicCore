using System.Collections.Generic;
using System.Linq;
using Core.Features.Data.Configs;
using Core.Loader.Features;
using MEC;
using Player = Exiled.API.Features.Player;
using Server = Exiled.Events.Handlers.Server;

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
        Timing.CallDelayed(1, ShowScp);
        Timing.CallDelayed(30, () => ScpSwapCommand.IsOpened = false);
    }

    private static void ShowScp()
    {
        HashSet<Player> scp = new();
        string msg = string.Empty;

        foreach (Player p in Player.List.Where(x => x.IsScp))
        {
            msg += $"| {p.Role.Type} | ";
            scp.Add(p);
        }

        foreach (Player scpP in scp)
            scpP.Broadcast(15, $@"\n<size=30><b>Don't you like your SCP?\nUse <color=#aaa>.scpswap</color> to swap it!\n<color=#EC2121><size=20>TEAMMATES:</size>\n{msg}</color>\n</b></size>");
    }
}