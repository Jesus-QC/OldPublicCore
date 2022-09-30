using System;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Display;
using Core.Features.Extensions;
using Core.Features.Wrappers;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
public class PinMessageCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("cursed.owner"))
        {
            response = "Perms";
            return false;
        }
        
        if (arguments.Count < 1)
        {
            GameDisplayBuilder.PinnedMessage = string.Empty;
            response = "cleared pin";
            return false;
        }

        string msg = "";
        for (int i = 0; i < arguments.Count; i++)
        {
            msg += arguments.At(i) + " ";
        }

        GameDisplayBuilder.PinnedMessage = msg;
        
        response = $"pinned: {msg}";
        return true;
    }

    public string Command { get; } = "pin";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Allows admins to pin a msg for any user.";
}