using System;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Modules.Levels;
using Exiled.API.Features;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(ClientCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
public class CoreVersionCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (Player.TryGet(sender, out var player))
        {
            player.SendHint(ScreenZone.Notifications, $"⌈ <color=#aeff70>◉</color> | Core Test Token | <color=#aeff70>+ 0XP</color> ⌋");
            player.SendHint(ScreenZone.Notifications, $"⌈ <color=#70b0ff>◈</color> | Core Test Token | <color=#70b0ff>+ 0XP</color> ⌋");
            player.SendHint(ScreenZone.Notifications, $"⌈ <color=#e070ff>❖</color> | Core Test Token | <color=#e070ff>+ 0XP</color> ⌋");
            player.SendHint(ScreenZone.Notifications, $"⌈ <color=#ffc670>✴</color> | Core Test Token | <color=#ffc670>+ 0XP</color> ⌋");
            player.SendHint(ScreenZone.Notifications, $"⌈ <color=#f7ff66>✽</color> | Core Test Token | <color=#f7ff66>+ 0XP</color> ⌋");
        }
        
        response = "The server is running the version: " + Core.GlobalVersion;
        return true;
    }

    public string Command { get; } = "coreversion";
    public string[] Aliases { get; } = { "corever" };
    public string Description { get; } = "Shows the server Core version.";
}