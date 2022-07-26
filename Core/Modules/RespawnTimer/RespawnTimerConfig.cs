using System.Collections.Generic;
using Exiled.API.Interfaces;

namespace Core.Modules.RespawnTimer
{
    public class RespawnTimerConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public List<string> Tips { get; set; } = new()
        {
            "W<lowercase>eapons deal more damage to <color=#154360>HUMANS</color> and less damage to <b><color=#641E16>SCPs</color>",
            "SCP 106 <lowercase>has <color=#641E16>MUCH</color> more health making him closer to the Lore.",
            "Y<lowercase>ou can <color=#ffdf4f>level up</color> by doing a lot of things like <color=#ff4f6c>killing</color> or <color=#69ff4f>farming</color>.",
            "Y<lowercase>ou can type <color=#F4D03F>.lvl lb</color> into the console to see the leaderboard.",
            "Y<lowercase>ou can <color=#F4D03F>GAMBLE</color> items at <color=#ff4f4f><uppercase>scp </uppercase>173</color> cell! It might help.",
            "<color=#4fff98>H<lowercase>uman</color> classes spawn with <color=#154360>CUSTOM</color> inventories.",
            "<color=#924fff>D<lowercase>ONT LOOK AWAY</color> from <uppercase><color=#ffa74f>SCP-173</color></uppercase> unless you want to die.",
            "SCP-096 <lowercase>is that tall skinny man. Opposite of <color=#ff4f4f>173</color>, do not look!",
            "N<lowercase>o clue how to play the role you are? <color=#F4D03F><uppercase>P</uppercase>ress F1</color> for a guide.",
            "D<lowercase>ont forget to read the <color=#F4D03F>SERVER INFO</color> for more info on the server.",
            "<color=#f96854> P<lowercase>atreon supporters</color> gets exclusive benefits on the server.",
            "<color=red>SCP 173</color> <lowercase>weakness is being seen by <color=#4f6fff>multiple</color> people.",
            "<color=#4fffa4>SCP-914</color> <lowercase>is one of the ways to <color=yellow>upgrade</color> your items or <color=#ff4f67>destroy</color> them...",
            "U<lowercase>sually chaos cooperate with <color=#4fffa4>SCPs</color>! But not always.",
            "S<lowercase>pawn tickets choose how many people <color=yellow>spawn</color>.",
            "Y<lowercase>ou can hold C to crouch walk without making sound.",
        };
    }
}