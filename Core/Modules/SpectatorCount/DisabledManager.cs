using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;

namespace Core.Modules.SpectatorCount;

public class DisabledManager
{
    private HashSet<string> _hiddenPlayers = new ();

    public void Load()
    {
        var path = Path.Combine(Paths.Configs, "Spectator List");
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

    public void Add(Player player)
    {
        var path = Path.Combine(Paths.Configs, "Spectator List", "hiddenlist.txt");
        File.WriteAllText(path, File.ReadAllText(path) + "\n" + player.UserId);
        _hiddenPlayers.Add(player.UserId);
    }

    public void Remove(Player player)
    {
        _hiddenPlayers.Remove(player.UserId);
        
        var path = Path.Combine(Paths.Configs, "Spectator List", "hiddenlist.txt");

        var newList = new List<string>();

        foreach (var line in File.ReadLines(path))
        {
            if(line != player.UserId)
                newList.Add(line);
        }
        
        File.WriteAllLines(path, newList);
    }

    public bool IsHidden(Player player)
    {
        return _hiddenPlayers.Contains(player.UserId);
    }
}