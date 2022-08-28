using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Core.Modules.Essentials;

public class EssentialsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public int RoundsToRestart { get; set; } = 1;
    
    [Description("Player Management")]
    public List<string> DisallowedWordsInName { get; set; } = new() { ".org" };
    public bool CanCuffedPlayersBeDamaged { get; set; } = false;

    [Description("Tesla Management")]
    public List<ItemType> ItemsThatDisablesTesla { get; set; } = new()
    {
        ItemType.KeycardScientist,
        ItemType.KeycardResearchCoordinator,
        ItemType.KeycardZoneManager,
        ItemType.KeycardGuard,
        ItemType.KeycardNTFOfficer,
        ItemType.KeycardContainmentEngineer,
        ItemType.KeycardNTFLieutenant,
        ItemType.KeycardNTFCommander,
        ItemType.KeycardFacilityManager,
        ItemType.KeycardO5,
    };

    [Description("Entance Announcements. %unit% for unit name, %unitnumber%, %scps% for number of scps. With chaos only %scps% work.")] 
    public string MtfAnnouncement { get; set; } = "Pitch_0.3 .g4 .g4 pitch_1 MTFUNIT Epsilon 11 .g1 designated ninetailedfox Pitch_1.5 .g3 Pitch_1 Pitch_0.9 HASENTERED Pitch_0.4 .g6 .g6";
    public string ChaosAnnouncement { get; set; } = "Pitch_0.3 .g4 .g4 Pitch_1 Emergency JAM_050_3  Alert , Unauthorized .g6 military group JAM_060_4";

    [Description("Flashlight chance")]
    public int FlashlightChance { get; set; } = 90;

    [Description("Disguise")]
    public List<string> DisguiseNicknames { get; set; } = new ()
    {
        "John Doe 1", "Cookiemonster871", "Monke 1", "Sky 1"
    };
}