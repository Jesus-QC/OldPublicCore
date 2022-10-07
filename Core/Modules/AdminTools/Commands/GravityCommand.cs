using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace Core.Modules.AdminTools.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GravityCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("cursed.owner"))
        {
            response = "Permissions not enough.";
            return false;
        }

        if (arguments.Count == 0)
        {
            Physics.gravity = Vector3.down * 9.8f;
            response = "gravity reseted";
            return true;
        }

        if (!float.TryParse(arguments.At(0), out float res))
        {
            response = "Couldn't parse the argument.";
            return false;
        }

        Physics.gravity = res * Vector3.up;
        response = "Done";
        return true;
    }

    public string Command { get; } = "gravity";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Manages gravity";
}