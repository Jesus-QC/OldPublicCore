using Exiled.API.Features;

namespace Core.Modules.Essentials.Extensions;

public static class PlayerExtensions
{
    public static bool HasIllegalName(this Player player)
    {
        foreach (var disallowedWord in EssentialsModule.PluginConfig.DisallowedWordsInName)
        {
            if (player.Nickname.Contains(disallowedWord))
                return true;
        }

        return false;
    }
}