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
    private CoroutineHandle _positionCoroutine;
    private GameObject _scp106Portal;
    private readonly HashSet<Player> _affectedPlayers  = new();

    private bool _isActivated = true;

    public void OnWaitingForPlayers()
    {
        _isActivated = true;
        _positionCoroutine = Timing.RunCoroutine(CheckPositions());
    }
    
    public void OnRestartingRound()
    {
        if (_positionCoroutine.IsRunning)
            Timing.KillCoroutines(_positionCoroutine);
    }
    
    public void OnStayingOnEnvironmentalHazard(StayingOnEnvironmentalHazardEventArgs ev)
    {
        if (ev.Player is null || ev.Player.IsScp)
            return;
        
        if (ev.EnvironmentalHazard is SinkholeEnvironmentalHazard && Vector3.Distance(ev.Player.Position, ev.EnvironmentalHazard.transform.position) < 3.5f)
        {
            Timing.RunCoroutine(PortalAnimation(ev.Player));
        }
    }

    public void OnCreatingPortal(CreatingPortalEventArgs ev)
    {
        _isActivated = false;
        Timing.CallDelayed(4, () => _isActivated = true);
    }
    
    private IEnumerator<float> CheckPositions()
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(0.1f);
            
            if (Warhead.IsDetonated)
                yield break;

            if (_scp106Portal != null)
            {
                if(!_isActivated)
                    continue;
                    
                foreach (var player in Player.List)
                {
                    if (player is null
                        || !player.IsAlive
                        || player.Role.Team == Team.SCP)
                        continue;

                    if (Vector3.Distance(player.Position, _scp106Portal.transform.position) <= 2.5f)
                    {
                        Timing.RunCoroutine(PortalAnimation(player));
                    }
                }
            }
            else
            {
                _scp106Portal = GameObject.Find("SCP106_PORTAL");
            }
        }
    }
    
    private IEnumerator<float> PortalAnimation(Player player)
    {
        if(_affectedPlayers.Contains(player))
            yield break;
        
        _affectedPlayers.Add(player);
        
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

        _affectedPlayers.Remove(player);
    }
}