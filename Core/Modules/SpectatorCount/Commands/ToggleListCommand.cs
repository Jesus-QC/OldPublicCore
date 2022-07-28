using System;
using CommandSystem;
using Exiled.API.Features;

namespace Core.Modules.SpectatorCount.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class ToggleListCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);

        if (player is null)
        {
            response = "Only players in-game can use this command!";
            return false;
        }

        if (SpectatorCountModule.DisabledManager.IsHidden(player))
        {
            SpectatorCountModule.DisabledManager.Remove(player);
            response = "Show list: true";
            return true;
        }
        
        SpectatorCountModule.DisabledManager.Add(player);
        response = "Show list: false";
        return true;
    }

    public string Command { get; } = "spectatorlist";
    public string[] Aliases { get; } = { };
    public string Description { get; } = "Toggles the spectator list.";
}