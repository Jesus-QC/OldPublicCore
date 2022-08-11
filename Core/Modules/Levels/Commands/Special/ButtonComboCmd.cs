using System;
using CommandSystem;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Modules.Levels.Commands.Special;

[CommandHandler(typeof(ClientCommandHandler))]
public class ButtonComboCmd : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (Player.TryGet(sender, out var ply))
        {
            if(ply.CheckCooldown(LevelToken.ButtonCombo, 1))
                ply.AddExp(LevelToken.ButtonCombo);
        }

        response = "Ch34t2 3n4bl3d";
        return true;
    }

    public string Command { get; } = "enablecheats1";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Enable cheats";
}