using System;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Features.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class HintZoneCmd : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var zone = (ScreenZone)int.Parse(arguments.At(0));
        var player = Player.Get(sender);

        player.SendHint(zone, arguments.At(1).Replace("ñ", "\n"));

        response = "";
        return true;
    }

    public string Command { get; } = "hintzone";
    public string[] Aliases { get; } = { };
    public string Description { get; } = "Testing command";
}