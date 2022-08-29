using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Modules.Stalky.Components;

public class StalkController : MonoBehaviour
{
    private float _cooldown = StalkyModule.ModuleConfig.InitialCooldown;
    private bool _inCooldown = true;

    private float _lastCreated = -1;

    private Player _player;

    private void Start()
    {
        _player = Player.Get(gameObject);
        StalkyModule.Controllers.Add(_player, this);
        _player.SendHint(ScreenZone.Center, "\n\n\n\nI<lowercase>n this server, scp <color=#ff8297>106</color> can <color=#82ff88>stalk</color>\n<uppercase>M</uppercase>ake <color=#82d5ff>two portals</color> to <color=#82ff88>stalk</color> humans</lowercase>", 20);
    }

    private void OnDestroy()
    {
        if(StalkyModule.Controllers.ContainsKey(_player))
            StalkyModule.Controllers.Remove(_player);
    }

    private void Update()
    {
        if (_player.Role != RoleType.Scp106)
        {
            StalkyModule.Controllers.Remove(_player);
            Destroy(this);
            return;
        }
            
        if (!_inCooldown)
            return;
            
        _cooldown -= Time.deltaTime;

        if (_cooldown > 0)
            return;
            
        _inCooldown = false;
        AnnounceReady();
    }

    private void AnnounceReady()
    {
        _player.SendHint(ScreenZone.Center, "\n\n\n\nyou can\n<b><color=#ff75a8>STALK</color></b>", 5);
    }

    private IEnumerator<float> AnnounceCooldown(int cooldown)
    {
        for (var i = 0; i < 5; i++)
        {
            if (cooldown - i < 0)
                yield break;
                
            _player.SendHint(ScreenZone.Center, $"\n\n\n\ncooldown\n<b><color=#f2fc92>{cooldown - i} seconds</color></b>",1);
            yield return Timing.WaitForSeconds(1f);
        }
    }
        
    public bool Stalk()
    {
        if (_lastCreated < 0 || Time.time - _lastCreated > 4)
        {
            _lastCreated = Time.time;
            return true;
        }

        if (_cooldown > 0)
        {
            Timing.RunCoroutine(AnnounceCooldown((int) _cooldown));
            return false;
        }
            
        var availablePlayers = new List<Player>();

        foreach (var player in Player.List)
            if ((player.Role.Team == Team.CDP || player.Role.Team == Team.MTF || player.Role.Team == Team.RSC) && player.CurrentRoom is not { Type: RoomType.Pocket })
                availablePlayers.Add(player);

        if (availablePlayers.Count == 0)
        {
            _player.SendHint(ScreenZone.Center, "\n\n\nThere are not\n<b><color=#ff6257>available players</color></b>", 4);
            return true;
        }

        var script = _player.ReferenceHub.scp106PlayerScript;

        var rndPlayer = availablePlayers[Random.Range(0, availablePlayers.Count)];

        if (rndPlayer is null)
        {
            _player.SendHint(ScreenZone.Center, "\n\n\nThere are not\n<b><color=#ff6257>available players</color></b>", 4);
            return true;
        }
            
        if (!Physics.Raycast(rndPlayer.Position, Vector3.down, out var raycastHit, 10f, script.teleportPlacementMask))
        {
            _player.SendHint(ScreenZone.Center, "\n\n\n<color=red>ERROR</color>", 3);
            return false;
        }

        Timing.RunCoroutine(StartStalk(raycastHit.point, rndPlayer.CurrentRoom));
        _player.SendHint(ScreenZone.Center, $"\n\n\n\nyou are stalking \n<b><color={rndPlayer.Role.Color.ToHex()}>{rndPlayer.Nickname}</color></b>", 5);

        _cooldown = StalkyModule.ModuleConfig.Cooldown;
        _inCooldown = true;
        return false;
    }

    private IEnumerator<float> StartStalk(Vector3 pos, Room room)
    {
        var script = _player.ReferenceHub.scp106PlayerScript;
            
        script.NetworkportalPosition = pos;

        do
        {
            script.UserCode_CmdUsePortal();
            yield return Timing.WaitForOneFrame;
        } 
        while (!script.goingViaThePortal);

        room.TurnOffLights(10);
            
        StalkyModule.AreTeslasEnabled = false;

        Timing.CallDelayed(10, () => StalkyModule.AreTeslasEnabled = true);
    }
}