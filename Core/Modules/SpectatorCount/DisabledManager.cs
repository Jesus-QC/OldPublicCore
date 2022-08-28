using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;

namespace Core.Modules.SpectatorCount;

public static class DisabledManager
{
    private static HashSet<string> _hiddenPlayers = new ();

    public static void Load()
    {
        var path = Path.Combine(Paths.Configs, "Core " + Server.Port, "Spectator List");
        Directory.CreateDirectory(path);

        var filePath = Path.Combine(path, "hiddenlist.txt");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "# This are the players with the spectator list hidden.");
        }

        foreach (var line in File.ReadLines(filePath))
        {
            if(line.StartsWith("#"))
                continue;

            _hiddenPlayers.Add(line);
        }
    }

    public static void Add(Player player)
    {
        var path = Path.Combine(Paths.Configs, "Core " + Server.Port, "Spectator List", "hiddenlist.txt");
        File.AppendAllText(path, '\n' + player.UserId);
        _hiddenPlayers.Add(player.UserId);
    }

    public static void Remove(Player player)
    {
        _hiddenPlayers.Remove(player.UserId);
        
        var path = Path.Combine(Paths.Configs, "Core " + Server.Port, "Spectator List", "hiddenlist.txt");

        var newList = new List<string>();

        foreach (var line in File.ReadLines(path))
        {
            if(line != player.UserId && !string.IsNullOrEmpty(line))
                newList.Add(line);
        }
        
        File.WriteAllLines(path, newList);
    }

    public static bool IsHidden(Player player)
    {
        return _hiddenPlayers.Contains(player.UserId);
    }
}