﻿using System;
using System.Collections.Generic;
using CommandSystem;
using Core.Features.Extensions;
using Core.Modules.Essentials;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Random = UnityEngine.Random;

namespace Core.Features.Commands;

[CommandHandler(typeof(ClientCommandHandler)), CommandHandler(typeof(RemoteAdminCommandHandler))]
public class DisguiseCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (Player.TryGet(sender, out Player player))
        {
            if (!sender.CheckPermission("staff.disguise"))
            {
                response = "You don't have perms to do that.";
                return false;
            }
        
            if (DisguisedStaff.ContainsKey(player.UserId))
            {
                DisguisedStaff.Remove(player.UserId);
                player.ShowBadge();
                
                response = "Undisguised.";
                return true;
            }

            if (arguments.Count > 0 && sender.CheckPermission("cursed.owner"))
            {
                DisguisedStaff.Add(player.UserId, arguments.At(0));
                player.ShowBadge();
            
                response = "Disguised.";
                return true;
            }
            
            DisguisedStaff.Add(player.UserId, EssentialsModule.PluginConfig.DisguiseNicknames[Random.Range(0, EssentialsModule.PluginConfig.DisguiseNicknames.Count)]);
            player.ShowBadge();
            
            response = "Disguised.";
            return true;
        }

        response = "Player not found.";
        return false;
    }

    public string Command { get; } = "disguise";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Disguises moderators with other nicknames";

    public static readonly Dictionary<string, string> DisguisedStaff = new ();
}