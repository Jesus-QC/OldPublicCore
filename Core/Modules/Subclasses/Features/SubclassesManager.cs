using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Attribute;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Subclasses.ClassD;
using Core.Modules.Subclasses.Features.Subclasses.Scientist;

namespace Core.Modules.Subclasses.Features;

public class SubclassesManager
{
    public void Load()
    {
        RegisterSubclass(typeof(Subclasses.ClassD.DefaultSubclass));
        RegisterSubclass(typeof(AdventurerSubclass));
        RegisterSubclass(typeof(CleanerSubclass));
        RegisterSubclass(typeof(CollectorSubclass));
        RegisterSubclass(typeof(DoctorSubclass));
        RegisterSubclass(typeof(FighterSubclass));
        RegisterSubclass(typeof(MidgetSubclass));
        
        RegisterSubclass(typeof(Subclasses.Scientist.DefaultSubclass));
        RegisterSubclass(typeof(InsiderSubclass));
        RegisterSubclass(typeof(RunnerSubclass));
    }

    public readonly Dictionary<int, Subclass> SubclassesById = new ();
    public readonly Dictionary<RoleType, Dictionary<CoreRarity, List<Subclass>>> SubclassesByRole = new();

    private void RegisterSubclass(Type type)
    {
        if(Activator.CreateInstance(type) is not Subclass subclass || type.GetCustomAttributes(typeof(DisabledFeatureAttribute), false).Any())
            return;

        var count = SubclassesById.Count;
        subclass.Id = count;
        SubclassesById.Add(count, subclass);

        foreach (var role in subclass.AffectedRoles)
        {
            if(!SubclassesByRole.ContainsKey(role))
                SubclassesByRole.Add(role, new Dictionary<CoreRarity, List<Subclass>>());

            if(!SubclassesByRole[role].ContainsKey(subclass.Rarity))
                SubclassesByRole[role].Add(subclass.Rarity, new List<Subclass>());

            SubclassesByRole[role][subclass.Rarity].Add(subclass);
        }
    }
}