using System;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace Core.Modules.AdminTools.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class VanishCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("cursed.owner"))
        {
            response = "Permissions not enough.";
            return false;
        }

        if (!Player.TryGet(sender, out Player player))
        {
            response = "player not found";
            return false;
        }
        
        player.ChangeAppearance(RoleType.Spectator);
        
        response = "Done";
        return true;
    }

    public string Command { get; } = "vanish";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Vanishes";
}