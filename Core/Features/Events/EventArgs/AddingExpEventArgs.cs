using Exiled.API.Features;

namespace Core.Features.Events.EventArgs;

public class AddingExpEventArgs : System.EventArgs
{
    public AddingExpEventArgs(Player player, int experienceAmount)
    {
        IsAllowed = true;
        Player = player;
        Amount = experienceAmount;
    }
        
    public bool IsAllowed { get; set; }
    public Player Player { get; }
    public int Amount { get; set; }
}