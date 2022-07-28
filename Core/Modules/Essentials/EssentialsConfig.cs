using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Core.Modules.Essentials;

public class EssentialsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    [Description("VoiceChat")]
    public List<RoleType> ScpsAbleToTalk { get; set; } = new() { RoleType.Scp049, RoleType.Scp079, RoleType.Scp096, RoleType.Scp106, RoleType.Scp173, RoleType.Scp0492, RoleType.Scp93953, RoleType.Scp93989 };

    [Description("Player Management")]
    public List<string> DisallowedWordsInName { get; set; } = new() { ".org" };
    public bool CanCuffedPlayersBeDamaged { get; set; } = false;

    [Description("Tesla Management")]
    public List<ItemType> ItemsThatDisablesTesla { get; set; } = new()
    {
        ItemType.KeycardJanitor,
        ItemType.KeycardChaosInsurgency
    };

    [Description("Entance Announcements. %unit% for unit name, %unitnumber%, %scps% for number of scps. With chaos only %scps% work.")] 
    public string MtfAnnouncement { get; set; } = "HELLO WORLD";
    public string ChaosAnnouncement { get; set; } = "HELLO WORLD";

    [Description("Flashlight chance")]
    public int FlashlightChance { get; set; } = 80;
}