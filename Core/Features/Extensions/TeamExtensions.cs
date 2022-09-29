namespace Core.Features.Extensions;

public static class TeamExtensions
{
    public static string GetLobbyName(this Team team)
    {
        switch (team)
        {
            case Team.CDP:
                return "<color=#FF8E00>ClassD</color>";
            case Team.SCP:
                return "<color=#EC2121>SCP</color>";
            case Team.MTF:
                return "<color=#0096FF>Guard</color>";
            case Team.RSC:
                return "<color=#FFFF7C>Scientist</color>";
            case Team.RIP:
                return "<color=#FC00AB>Overwatch</color>";
        }

        return "<color=#FC00AB>Random</color>";
    }
}