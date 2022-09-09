using System;
using System.Linq;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Modules.Levels.Commands.Special;

[CommandHandler(typeof(ClientCommandHandler))]
public class WolfPackCmd : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (Player.TryGet(sender, out Player ply))
        {
            string achievements = ply.GetSpecialAdvancements();
            if (!achievements.Contains('0'))
            {
                ply.AddExp(LevelToken.WolfPackForever, 1000);
                ply.SaveSpecialAdvancements(achievements+'0');
            }
        }

        response = "Wolf pack 4 ever.";
        return true;
    }

    public string Command { get; } = "wolfpack";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Wolf pack command.";
}