using System;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Features.Wrappers;
using Exiled.API.Features;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
public class DisableHudCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 1)
        {
            response = "no enough arguments";
            return false;
        }

        if (arguments.At(0) is "off")
        {
            MapCore.IsHudEnabled = false;
            response = "disabled hud.";
            return false;
        }

        MapCore.IsHudEnabled = true;
        response = "enabled hud";
        return true;
    }

    public string Command { get; } = "disablehud";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Allows admins to disable the hud for any user.";
}