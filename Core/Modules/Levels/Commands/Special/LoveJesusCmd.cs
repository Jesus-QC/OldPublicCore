using System;
using System.Linq;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Modules.Levels.Commands.Special;

[CommandHandler(typeof(ClientCommandHandler))]
public class LoveJesusCmd : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (Player.TryGet(sender, out var ply))
        {
            var achievements = ply.GetSpecialAdvancements();
            if (!achievements.Contains('1'))
            {
                ply.AddExp(LevelToken.JesusSupportCode);
                ply.SaveSpecialAdvancements(achievements+'1');
            }
        }

        response = "Jesus loves you 2.";
        return true;
    }

    public string Command { get; } = "lovejesus";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Love jesus command.";
}