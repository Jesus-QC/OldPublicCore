using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Attribute;
using Core.Features.Data.Enums;
using Core.Features.Logger;
using Core.Modules.Subclasses.Features.Enums;
using Core.Modules.Subclasses.Features.Subclasses.ClassD;
using Core.Modules.Subclasses.Features.Subclasses.Scientist;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features;

public class SubclassesManager
{
    public void Load()
    {
        RegisterSubclass(typeof(Subclasses.ClassD.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.AdventurerSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.CleanerSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.CollectorSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.DoctorSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.FighterSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.MidgetSubclass));
        
        RegisterSubclass(typeof(Subclasses.Scientist.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.InsiderSubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.RunnerSubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.ChaosSpySubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.EngineerSubclass));
        
        RegisterSubclass(typeof(Subclasses.Guard.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.Guard.BreacherSubclass));
        RegisterSubclass(typeof(Subclasses.Guard.GrenadierSubclass));
        RegisterSubclass(typeof(Subclasses.Guard.FacilityManagerSubclass));
        
        RegisterSubclass(typeof(Subclasses.MTF.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.HackerSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.MtfSpySubclass));
        RegisterSubclass(typeof(Subclasses.MTF.ReconSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.SniperSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.SpecialistSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.TankSubclass));
        
        RegisterSubclass(typeof(Subclasses.Chaos.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.Chaos.ChaosJuggernautSubclass));
        RegisterSubclass(typeof(Subclasses.Chaos.JammerSubclass));
        RegisterSubclass(typeof(Subclasses.Chaos.DynamiterSubclass));
    }

    public readonly Dictionary<int, Subclass> SubclassesById = new ();
    public readonly Dictionary<RoleType, Dictionary<CoreRarity, List<Subclass>>> SubclassesByRole = new();

    private void RegisterSubclass(Type type)
    {
        if(Activator.CreateInstance(type) is not Subclass subclass || type.GetCustomAttributes(typeof(DisabledFeatureAttribute), false).Any())
            return;

        var count = SubclassesById.Count;
        subclass.Id = count;
        
        subclass.TopBar = subclass.Color is null
            ? $"subclass: {subclass.Name} ({subclass.Rarity.ToString().ToLower()})"
            : $"subclass: <color={subclass.Color}>{subclass.Name} ({subclass.Rarity.ToString().ToLower()})</color>";
        subclass.SecondaryTopBar = "abilities: " + subclass.Abilities.ToString().ToLower();
        
        SubclassesById.Add(count, subclass);

        foreach (var role in subclass.AffectedRoles)
        {
            if(!SubclassesByRole.ContainsKey(role))
                SubclassesByRole.Add(role, new Dictionary<CoreRarity, List<Subclass>>());

            if(!SubclassesByRole[role].ContainsKey(subclass.Rarity))
                SubclassesByRole[role].Add(subclass.Rarity, new List<Subclass>());

            SubclassesByRole[role][subclass.Rarity].Add(subclass);
        }
        
        Log.Info($"Subclass: {LogUtils.GetColor(LogColor.Magenta)}{subclass.Name} {LogUtils.GetColor(LogColor.Cyan)}has been registered!");
    }
}