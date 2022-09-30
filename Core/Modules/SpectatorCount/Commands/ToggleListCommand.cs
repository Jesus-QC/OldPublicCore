using System;
using CommandSystem;
using Exiled.API.Features;

namespace Core.Modules.SpectatorCount.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class ToggleListCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player player = Player.Get(sender);

        if (player is null)
        {
            response = "Only players in-game can use this command!";
            return false;
        }

        if (DisabledManager.IsHidden(player))
        {
            DisabledManager.Remove(player);
            response = "Show list: true";
            return true;
        }
        
        DisabledManager.Add(player);
        response = "Show list: false";
        return true;
    }

    public string Command { get; } = "spectatorlist";
    public string[] Aliases { get; } = {"spectatorcount"};
    public string Description { get; } = "Toggles the spectator list.";
}