using System;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
public class CoreHintSendCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 2)
        {
            response = "no enough arguments";
            return false;
        }

        if (!Player.TryGet(arguments.At(0), out var ply))
        {
            response = "Player not found";
            return false;
        }

        var zone = (ScreenZone)int.Parse(arguments.At(1));

        var msg = "";
        for (int i = 2; i < arguments.Count; i++)
        {
            msg += arguments.At(i);
        }
        
        ply.SendHint(zone, msg);
        
        response = "Sent hint to player " + ply.Nickname;
        return true;
    }

    public string Command { get; } = "sendcorehint";
    public string[] Aliases { get; } = { "sch" };
    public string Description { get; } = "Sends a hint to a player using core system.";
}