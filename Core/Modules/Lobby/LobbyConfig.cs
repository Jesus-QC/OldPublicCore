using Exiled.API.Interfaces;

namespace Core.Modules.Lobby;

public class LobbyConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public string ServerAnnouncement { get; set; } = "<b><i>We are currently looking for new staff, join the <color=#5865F2>Discord</color> server to apply.</i></b>";
}