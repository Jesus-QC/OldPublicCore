using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Extensions;

public static class RarityExtensions
{
    public static CoreRarity GetRandomRarity()
    {
        int value = Random.Range(0, 101);

        return value switch
        {
            <= 1 => CoreRarity.Mythic,
            <= 6 => CoreRarity.Legendary,
            <= 17 => CoreRarity.Epic,
            <= 34 => CoreRarity.Rare,
            _ => CoreRarity.Common // 42%
        };
    }
}