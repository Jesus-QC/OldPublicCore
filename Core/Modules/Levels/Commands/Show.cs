using System;
using CommandSystem;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Modules.Levels.Commands;

public class Show : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player ply = Player.Get(sender);
        if (ply == null || ply == Server.Host)
        {
            response = "You have to be in-game";
            return false;
        }

        int lvl = ply.GetLevel();
            
        response = $"Your level is {lvl}.";
        return true;
    }

    public string Command { get; } = "show";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Shows you your level and exp for leveling up.";
}