using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Attribute;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Features.Logger;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features;

public class SubclassesManager
{
    public bool IsEnabled = true;
    
    public void Load()
    {
        RegisterSubclass(typeof(Subclasses.ClassD.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.AdventurerSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.CleanerSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.CollectorSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.DoctorSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.FighterSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.ChadSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.MidgetSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.HustlerSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.ChadSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.BaddieSubclass));
        RegisterSubclass(typeof(Subclasses.ClassD.HalloweenEnjoyer));
        
        RegisterSubclass(typeof(Subclasses.Scientist.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.InsiderSubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.RunnerSubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.ChaosSpySubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.EngineerSubclass));
        RegisterSubclass(typeof(Subclasses.Scientist.MrWhiteSubclass));
        
        RegisterSubclass(typeof(Subclasses.Guard.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.Guard.BreacherSubclass));
        RegisterSubclass(typeof(Subclasses.Guard.GrenadierSubclass));
        RegisterSubclass(typeof(Subclasses.Guard.CommanderSubclass));
        
        RegisterSubclass(typeof(Subclasses.MTF.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.HackerSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.MtfSpySubclass));
        RegisterSubclass(typeof(Subclasses.MTF.ReconSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.SniperSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.SpecialistSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.TankSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.BomberSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.SlayerSubclass));
        RegisterSubclass(typeof(Subclasses.MTF.NightSpecialistSubclass));

        RegisterSubclass(typeof(Subclasses.Chaos.DefaultSubclass));
        RegisterSubclass(typeof(Subclasses.Chaos.ChaosJuggernautSubclass));
        RegisterSubclass(typeof(Subclasses.Chaos.JammerSubclass));
        RegisterSubclass(typeof(Subclasses.Chaos.DynamiterSubclass));
        RegisterSubclass(typeof(Subclasses.Chaos.PeaceBreakerSubclass));
    }

    public readonly Dictionary<int, Subclass> SubclassesById = new ();
    public readonly Dictionary<RoleType, Dictionary<CoreRarity, List<Subclass>>> SubclassesByRole = new();

    private void RegisterSubclass(Type type)
    {
        if(Activator.CreateInstance(type) is not Subclass subclass || type.GetCustomAttributes(typeof(DisabledFeatureAttribute), false).Any())
            return;

        int count = SubclassesById.Count;
        subclass.Id = count;

        subclass.TopBar = subclass.Color is null
            ? $"subclass: {subclass.Name} ({subclass.Rarity.GetIcon()})"
            : $"subclass: <color={subclass.Color}>{subclass.Name} ({subclass.Rarity.GetIcon()})</color>";
        
        subclass.SecondaryTopBar = "abilities: " + subclass.Abilities.ToString().ToLower();
        
        if (subclass.MainAbility is not null)
        {
            subclass.TopBar += " | abilities " + subclass.Abilities.ToString().ToLower();
            subclass.SecondaryTopBar = "main: " + subclass.MainAbility.Ability.ToString().ToLower();

            if (subclass.SecondaryAbility is not null)
            {
                subclass.SecondaryTopBar += " | secondary: " + subclass.SecondaryAbility.Ability.ToString().ToLower();

                if (subclass.TertiaryAbility is not null)
                    subclass.SecondaryTopBar += " | tertiary: " + subclass.TertiaryAbility.Ability.ToString().ToLower();
            }
        }
        
        SubclassesById.Add(count, subclass);

        foreach (RoleType role in subclass.AffectedRoles)
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