﻿using System;
using System.Collections.Generic;
using CommandSystem;
using Core.Features.Extensions;
using Exiled.API.Features;
using MySqlConnector;

namespace Core.Modules.Levels.Commands;

public class Leaderboard : ICommand
{
    private static string _lb = string.Empty;
    private static DateTime _lastUpdate = DateTime.MinValue;
    private static List<string> _colorList = new() { "#4A235A", "#641E16", "#7D6608", "#1B4F72", "#186A3B" };
        
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player ply = Player.Get(sender);
        if (ply == null || ply == Server.Host)
        {
            response = "You have to be in-game";
            return false;
        }

        UpdateLb();
        ply.Broadcast(10, _lb);
            
        response = "There is a broadcast showing you the leaderboard.";
        return true;
    }

    private static void UpdateLb()
    {
        if(_lastUpdate >= DateTime.Now.AddMinutes(-5)) return;
            
        _lastUpdate = DateTime.Now;
            
        string msg = "<size=25%><i>LEVEL</i><b>LEADERBOARD</b>";
        
        using MySqlConnection anotherCon = Core.Database.Connection.Clone();
        anotherCon.Open();
        using MySqlCommand cmd = new("SELECT PlayerId, Exp FROM Leveling ORDER BY Exp DESC LIMIT 5", anotherCon);
            
        int number = 0;

        using MySqlConnection newCon = Core.Database.Connection.Clone();
        newCon.Open();
            
        using MySqlDataReader obj = cmd.ExecuteReader();
            
        if (obj.HasRows)
        {
            while (obj.Read())
            {
                object name = new MySqlCommand($"SELECT Username FROM NewPlayers WHERE Id='{obj.GetValue(0)}'", newCon).ExecuteScalar();
                msg += $"\n<b>#{number}</b> <i><color={_colorList[number]}>{name}</color></i> with <i><color=#ffe81c>{obj.GetValue(1)}</color></i> exp ({LevelExtensions.GetLevel((int)obj.GetValue(1))})";
                number++;
            }
        }

        msg += "\n</size>";
        _lb = msg;
    }

    public string Command { get; } = "leaderboard";
    public string[] Aliases { get; } = {"lb"};
    public string Description { get; } = "Shows you the global level leaderboard.";
}