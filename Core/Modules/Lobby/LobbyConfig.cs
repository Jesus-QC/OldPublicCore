using Exiled.API.Interfaces;

namespace Core.Modules.Lobby
{
    public class LobbyConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string ServerName { get; set; } = "<color=#E24B62>T</color><color=#E24E65>h</color><color=#E35269>e</color><color=#E3566C>W</color><color=#E45A70>o</color><color=#E55E73>l</color><color=#E56277>f</color><color=#E6667A>P</color><color=#E76A7E>a</color><color=#E76E81>c</color><color=#E87285>k</color>";
        public string DiscordLink { get; set; } = "<color=#E24B62>d</color><color=#E24D64>i</color><color=#E24F66>s</color><color=#E35268>c</color><color=#E3546A>o</color><color=#E3566C>r</color><color=#E4596F>d</color><color=#E45B71>.</color><color=#E55E73>g</color><color=#E56075>g</color><color=#E56277>/</color><color=#E66579>3</color><color=#E6677C>G</color><color=#E76A7E>j</color><color=#E76C80>Z</color><color=#E76E82>7</color><color=#E87184>B</color><color=#E87386>Z</color>";
        
        public string ServerPaused { get; set; } = "<color=#E2B24B>s</color><color=#E3B64B>e</color><color=#E4BB4A>r</color><color=#E6C04A>v</color><color=#E7C549>e</color><color=#E9CA49>r</color> <color=#ECD348>p</color><color=#EDD847>a</color><color=#EEDD46>u</color><color=#F0E246>s</color><color=#F1E645>e</color><color=#F3EB45>d</color><color=#F4F044>.</color><color=#F6F544>.</color><color=#F7FA43>.</color>";
        public string RoundStarting { get; set; } = "<color=#4BE269>r</color><color=#4BE36F>o</color><color=#4AE575>u</color><color=#4AE77C>n</color><color=#49E982>d</color> <color=#48ED8F>s</color><color=#47EF96>t</color><color=#47F19C>a</color><color=#46F3A3>r</color><color=#45F5A9>t</color><color=#45F7B0>i</color><color=#44F9B6>n</color><color=#44FBBD>g</color><color=#43FDC3>!</color>";
        public string SecondsRemain { get; set; } = "<color=#4BBCE2>s</color><color=#4BB9E4>e</color><color=#4AB6E6>c</color><color=#4AB3E8>o</color><color=#49AFEA>n</color><color=#48ACEC>d</color><color=#48A9EE>s</color> <color=#46A2F2>r</color><color=#469FF4>e</color><color=#459CF6>m</color><color=#4498F8>a</color><color=#4495FA>i</color><color=#4392FC>n</color>";

        public string TeamSelected { get; set; } = "<b><color=#F901D4>T</color><color=#F905CF>e</color><color=#F90AC9>a</color><color=#FA0EC3>m</color> <color=#FB17B7>S</color><color=#FB1CB1>e</color><color=#FC20AB>l</color><color=#FC25A5>e</color><color=#FC299F>c</color><color=#FD2E99>t</color><color=#FD3293>e</color><color=#FE378D>d</color><color=#FE3B87>:</color></b>";
        public string MtfSelected { get; set; } = "<color=#0196F9>M</color><color=#168EFB>T</color><color=#2C86FD>F</color>";
        public string ScientistsSelected { get; set; } = "<color=#F9DC01>S</color><color=#F9DC07>c</color><color=#FADC0E>i</color><color=#FADC14>e</color><color=#FBDB1B>n</color><color=#FCDB21>t</color><color=#FCDB28>i</color><color=#FDDA2E>s</color><color=#FDDA35>t</color><color=#FEDA3B>s</color>";
        public string ClassDSelected { get; set; } = "<color=#F98D01>C</color><color=#F98E0A>l</color><color=#FA8F13>a</color><color=#FB911C>s</color><color=#FC9226>s</color> <color=#FE9538>D</color>";
        public string ScpsSelected { get; set; } = "<color=#F90101>S</color><color=#FB162B>C</color><color=#FD2B56>P</color>";
    }
}