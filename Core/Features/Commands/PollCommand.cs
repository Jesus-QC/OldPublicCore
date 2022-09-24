using System;
using System.Linq;
using CommandSystem;
using Core.Features.Handlers;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class PollCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("cursed.owner"))
        {
            response = "You don't have perms to do that.";
            return false;
        }

        if (arguments.At(0) == "stop")
        {
            PollHandler.Enabled = false;
            response = "Ended poll";
            return true;
        }
        
        PollHandler.AddPoll("dedicated server", arguments.Aggregate("", (current, s) => current + (s + ' ')), 20, () => {Log.Info("Poll ended.");});
        
        response = "Done!";
        return true;
    }

    public string Command { get; } = "poll";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Starts a poll";
}