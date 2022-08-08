using Exiled.API.Features;

namespace Core.Features.Events.EventArgs;

public class WarnedEventArgs : System.EventArgs
{
    public WarnedEventArgs(Player player, Player issuer, string reason)
    {
        Player = player;
        Issuer = issuer;
        Reason = reason;
    }
        
    public Player Player { get; }
    public Player Issuer { get; }
    public string Reason { get; set; }
}