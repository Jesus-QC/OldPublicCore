using System;
using System.Collections.Generic;
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
        if (Player.TryGet(sender, out Player ply))
        {
            if (!_alreadyUsed.Contains(ply.UserId))
            {
                _alreadyUsed.Add(ply.UserId);
                ply.AddExp(LevelToken.ButtonCombo, 100);
            }
        }

        response = "Ch34t2 3n4bl3d";
        return true;
    }

    private static readonly HashSet<string> _alreadyUsed = new ();

    public string Command { get; } = "enablecheats1";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Enable cheats";
}