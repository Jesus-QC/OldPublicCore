using Exiled.API.Interfaces;

namespace Core.Modules.Lobby;

public class LobbyConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public string ServerAnnouncement { get; set; } = "<b><i><color=#3af26b>We are currently looking for new staff, join the <color=#5865F2>Discord</color> server to apply.</color></i></b>";
    public string ServerName { get; set; } = "<color=#ff3995>C</color><color=#f6479f>u</color><color=#ed55a8>r</color><color=#e463b2>s</color><color=#db71bc>e</color><color=#d27fc5>d</color>";
    public string DiscordLink { get; set; } = "<color=#ff3995>d</color><color=#ea59ab>i</color><color=#d579c2>s</color><color=#c09ad8>c</color><color=#acbaee>o</color><color=#a1caf9>r</color><color=#b6aae3>d</color><color=#cb8acd>.</color><color=#e069b6>c</color><color=#f549a0>u</color><color=#f549a0>r</color><color=#e069b6>s</color><color=#cb8acd>e</color><color=#b6aae3>d</color><color=#a1caf9>s</color><color=#acbaee>l</color><color=#c09ad8>.</color><color=#d579c2>x</color><color=#ea59ab>y</color><color=#ff3995>z</color>";
        
    public string ServerPaused { get; set; } = "<color=#E2B24B>s</color><color=#E3B64B>e</color><color=#E4BB4A>r</color><color=#E6C04A>v</color><color=#E7C549>e</color><color=#E9CA49>r</color> <color=#ECD348>p</color><color=#EDD847>a</color><color=#EEDD46>u</color><color=#F0E246>s</color><color=#F1E645>e</color><color=#F3EB45>d</color><color=#F4F044>.</color><color=#F6F544>.</color><color=#F7FA43>.</color>";
    public string RoundStarting { get; set; } = "<color=#4BE269>r</color><color=#4BE36F>o</color><color=#4AE575>u</color><color=#4AE77C>n</color><color=#49E982>d</color> <color=#48ED8F>s</color><color=#47EF96>t</color><color=#47F19C>a</color><color=#46F3A3>r</color><color=#45F5A9>t</color><color=#45F7B0>i</color><color=#44F9B6>n</color><color=#44FBBD>g</color><color=#43FDC3>!</color>";
    public string SecondsRemain { get; set; } = "<color=#4BBCE2>s</color><color=#4BB9E4>e</color><color=#4AB6E6>c</color><color=#4AB3E8>o</color><color=#49AFEA>n</color><color=#48ACEC>d</color><color=#48A9EE>s</color> <color=#46A2F2>r</color><color=#469FF4>e</color><color=#459CF6>m</color><color=#4498F8>a</color><color=#4495FA>i</color><color=#4392FC>n</color>";

    public string MtfSelected { get; set; } = "<color=#5B6370>Guard</color>";
    public string ScientistsSelected { get; set; } = "<color=#FFFF7C>Scientist</color>";
    public string ClassDSelected { get; set; } = "<color=#FF8E00>Class-D</color>";
    public string ScpsSelected { get; set; } = "<color=#EC2121>SCP</color>";
}