using System;
using CommandSystem;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Core.Modules.Subclasses.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SubclassCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("cursed.owner"))
        {
            response = "You don't have perms to do that.";
            return false;
        }
        
        if (arguments.Count != 1)
        {
            response = "Usage: subclass on/off";
            return false;
        }

        if (arguments.At(0) is "off")
        {
            SubclassesModule.SubclassesManager.IsEnabled = false;
            response = "Turned off";
            return true;
        }
        
        SubclassesModule.SubclassesManager.IsEnabled = true;
        response = "Done";
        return true;
    }

    public string Command { get; } = "subclass";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Subclass ability.";
}