using System.Collections.Generic;
using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Extensions;

public static class RoleExtensions
{
    public static Subclass GetRandomSubclass(this RoleType role)
    {
        if (!SubclassesModule.SubclassesManager.IsEnabled)
            return null;
        
        Dictionary<RoleType, Dictionary<CoreRarity, List<Subclass>>> dic = SubclassesModule.SubclassesManager.SubclassesByRole;

        if (!dic.ContainsKey(role))
            return null;

        CoreRarity randomRarity = RarityExtensions.GetRandomRarity();

        if (dic[role].ContainsKey(randomRarity))
        {
            List<Subclass> list = dic[role][randomRarity];
            return list[Random.Range(0, list.Count)];
        }

        if (!dic[role].ContainsKey(CoreRarity.Common))
            return null;
        
        List<Subclass> finalList = dic[role][CoreRarity.Common];
        return finalList[Random.Range(0, finalList.Count)];
    }
}