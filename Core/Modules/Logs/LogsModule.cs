using Core.Loader.Features;
using Core.Modules.Logs.Enums;
using Exiled.Events.EventArgs;
using HarmonyLib;
using MEC;
using RemoteAdmin;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.Logs;

public class LogsModule : CoreModule<LogsConfig>
{
    public override string Name { get; } = "Logs";

    public static LogsConfig LogsConfig;

    public override void OnEnabled()
    {
        LogsConfig = Config;

        WebhookSender.AddMessage("`SERVER CONNECTED ✨`", WebhookType.GameLogs);
            
        Timing.RunCoroutine(WebhookSender.ManageQueue());

        Player.Verified += OnVerified;
        Player.Left += OnLeft;
        Player.Handcuffing += OnHandcuffing;
        Player.Died += OnDied;

        Server.RoundEnded += OnRoundEnded;
        Server.RoundStarted += OnRoundStarted;
            
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        LogsConfig = null;

        base.OnDisabled();
    }

    private void OnVerified(VerifiedEventArgs ev) => WebhookSender.AddMessage($"`Join ✨` >> {ev.Player.Nickname.DiscordParse()} ({ev.Player.UserId}) [{ev.Player.IPAddress}]", WebhookType.GameLogs);
    private void OnLeft(LeftEventArgs ev) => WebhookSender.AddMessage($"`Left ⛔` >> {ev.Player.Nickname.DiscordParse()} ({ev.Player.UserId}) [{ev.Player.IPAddress}] as {ev.Player.Role}", WebhookType.GameLogs); 

    private void OnHandcuffing(HandcuffingEventArgs ev) => WebhookSender.AddMessage($"`Disarmed 🗝️` {ev.Cuffer.Nickname.DiscordParse()} ({ev.Cuffer.Role}) disarmed {ev.Target.Nickname.DiscordParse()} ({ev.Target.Role})", WebhookType.GameLogs);
    
    private void OnDied(DiedEventArgs ev)
    {
        if (ev.Killer is null || ev.Target is null)
            return;

        WebhookSender.AddMessage($"`Died ☠` {ev.Killer.Nickname.DiscordParse()} ({ev.Killer.Role}) killed {ev.Target.Nickname.DiscordParse()} ({ev.TargetOldRole}) with {ev.Handler.Type}", WebhookType.KillLogs);
    }

    private void OnRoundStarted() => WebhookSender.AddMessage("`🏁🟢🏁 New round has started!`", WebhookType.GameLogs);
    private void OnRoundEnded(RoundEndedEventArgs _) => WebhookSender.AddMessage("`🏁🔴🏁 The round has ended!`", WebhookType.GameLogs);  
}