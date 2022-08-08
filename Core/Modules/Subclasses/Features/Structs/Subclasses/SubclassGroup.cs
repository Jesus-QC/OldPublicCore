using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Structs.Subclasses;

public class SubclassGroup
{
    private readonly Dictionary<Rarity, Dictionary<ushort, Subclass>> _availableSubclasses = new() {[Rarity.Common] = new Dictionary<ushort, Subclass>(), [Rarity.Uncommon] = new Dictionary<ushort, Subclass>(), [Rarity.Rare] = new Dictionary<ushort, Subclass>(), [Rarity.Epic] = new Dictionary<ushort, Subclass>(), [Rarity.Legendary] = new Dictionary<ushort, Subclass>(), [Rarity.Mythic] = new Dictionary<ushort, Subclass>()};

    public void AddSubclass(ushort id, Subclass s) => _availableSubclasses[s.Rarity].Add(id, s);

    public Subclass GetRandomSubclass(Rarity rarity, out ushort id)
    {
        var available = _availableSubclasses[rarity];
        var amount = available.Count;
            
        if (amount < 1)
        {
            id = 9999;
            return null;
        }

        var sub = _availableSubclasses[rarity].ElementAt(Random.Range(0, amount));

        id = sub.Key;
        return sub.Value;
    }
}