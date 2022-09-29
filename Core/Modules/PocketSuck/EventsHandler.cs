using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
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
    private readonly HashSet<Player> _affectedPlayers  = new();

    public void OnStayingOnEnvironmentalHazard(StayingOnEnvironmentalHazardEventArgs ev)
    {
        if (ev.Player is null || ev.Player.IsScp)
            return;
        
        if (ev.EnvironmentalHazard is SinkholeEnvironmentalHazard && Vector3.Distance(ev.Player.Position, ev.EnvironmentalHazard.transform.position) < 3.5f)
        {
            Timing.RunCoroutine(PortalAnimation(ev.Player));
        }
    }

    private IEnumerator<float> PortalAnimation(Player player)
    {
        if(_affectedPlayers.Contains(player))
            yield break;
        
        _affectedPlayers.Add(player);
        
        bool inGodMode = player.IsGodModeEnabled;
        player.IsGodModeEnabled = true;
        player.CanSendInputs = false;

        player.ReferenceHub.scp106PlayerScript.GrabbedPosition = player.Position + (Vector3.up * 1.5f);
        Vector3 startPosition = player.Position, endPosition = player.Position -= Vector3.up * 1.23f * player.GameObject.transform.localScale.y;
        for (int i = 0; i < 30; i++)
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
        player.SendHint(ScreenZone.InteractionMessage, "<b>you have been sucked by a sinkhole</b>");
    }
}