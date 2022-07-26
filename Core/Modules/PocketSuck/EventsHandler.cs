using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using PlayerStatsSystem;
using UnityEngine;

namespace Core.Modules.PocketSuck;

public class EventsHandler
{
    private CoroutineHandle PositionCoroutine { get; set; }
    private GameObject Scp106Portal { get; set; }

    private HashSet<Player> AffectedPlayers { get; set; } = new();

    public void OnWaitingForPlayers()
    {
        PositionCoroutine = Timing.RunCoroutine(CheckPositions());
    }
    
    public void OnRestartingRound()
    {
        if (PositionCoroutine.IsRunning)
            Timing.KillCoroutines(PositionCoroutine);
    }
    
    public void OnStayingOnEnvironmentalHazard(StayingOnEnvironmentalHazardEventArgs ev)
    {
        if (ev.EnvironmentalHazard is SinkholeEnvironmentalHazard && Vector3.Distance(ev.Player.Position, ev.EnvironmentalHazard.transform.position) < 3.5f)
        {
            Timing.RunCoroutine(PortalAnimation(ev.Player));
        }
    }
    
    private IEnumerator<float> CheckPositions()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(0.1f);
            
            if (Warhead.IsDetonated)
                yield break;

            if (Scp106Portal != null)
            {
                foreach (var player in Player.List)
                {
                    if (player is null
                        || !player.IsAlive
                        || player.Role.Team == Team.SCP)
                        continue;

                    if (Vector3.Distance(player.Position, Scp106Portal.transform.position) <= 2.5f)
                    {
                        Timing.RunCoroutine(PortalAnimation(player));
                    }
                }
            }
            else
            {
                Scp106Portal = GameObject.Find("SCP106_PORTAL");
            }
        }
    }
    
    private IEnumerator<float> PortalAnimation(Player player)
    {
        if(AffectedPlayers.Contains(player))
            yield break;
        
        AffectedPlayers.Add(player);
        
        var inGodMode = player.IsGodModeEnabled;
        player.IsGodModeEnabled = true;
        player.CanSendInputs = false;

        player.ReferenceHub.scp106PlayerScript.GrabbedPosition = player.Position + (Vector3.up * 1.5f);
        Vector3 startPosition = player.Position, endPosition = player.Position -= Vector3.up * 1.23f * player.GameObject.transform.localScale.y;
        for (var i = 0; i < 30; i++)
        {
            player.Position = Vector3.Lerp(startPosition, endPosition, i / 30f);
            yield return 0f;
        }

        player.Position = Vector3.down * 1997f;
        player.IsGodModeEnabled = inGodMode;
        player.CanSendInputs = true;
        
        if (Warhead.IsDetonated)
        {
            player.Kill(DeathTranslations.PocketDecay.LogLabel);
            yield break;
        }

        player.Hurt(10, DamageType.PocketDimension);
        player.EnableEffect<Corroding>();

        AffectedPlayers.Remove(player);
    }
}