using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Extensions;

public static class RoleExtensions
{
    public static Subclass GetRandomSubclass(this RoleType role)
    {
        var dic = SubclassesModule.SubclassesManager.SubclassesByRole;

        if (!dic.ContainsKey(role))
            return null;

        var randomRarity = RarityExtensions.GetRandomRarity();

        if (dic[role].ContainsKey(randomRarity))
        {
            var list = dic[role][randomRarity];
            return list[Random.Range(0, list.Count)];
        }
        
        var finalList = dic[role][CoreRarity.Common];
        return finalList[Random.Range(0, finalList.Count)];
    }
}