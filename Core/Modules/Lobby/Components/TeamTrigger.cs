using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Lobby.Components;

public class TeamTrigger : MonoBehaviour
{
    public Team team;
    private string _name;
        
    private readonly List<Player> _players = new();

    public void Start()
    {
        var col = gameObject.AddComponent<CapsuleCollider>();
        col.isTrigger = true;

        _name = $"<size=150%>{GetTeamName(team)}</size>";
    }

    private float _counter;
        
    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter > 0.98f)
        {
            foreach (var player in _players)
            {
                player.SendHint(ScreenZone.Center, _name, 2);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var ply = Player.Get(other.gameObject);
        if (ply is null || _players.Contains(ply))
            return;

        _players.Add(ply);
        ply.SendHint(ScreenZone.Center, _name, 2);
    }

    private void OnTriggerExit(Collider other)
    {
        var ply = Player.Get(other.gameObject);
            
        if (ply is null)
            return;

        if (_players.Contains(ply))
        {
            _players.Remove(ply);
            ply.ClearHint(ScreenZone.Center);
        }
    }

    private static string GetTeamName(Team t)
    {
        switch (t)
        {
            case Team.MTF:
                return LobbyModule.LobbyConfig.MtfSelected;
            case Team.CDP:
                return LobbyModule.LobbyConfig.ClassDSelected;
            case Team.RSC:
                return LobbyModule.LobbyConfig.ScientistsSelected;
            case Team.SCP:
                return LobbyModule.LobbyConfig.ScpsSelected;
            default:
                return "Invalid";
        }
    }

    public bool ContainsPlayer(Player player) => _players.Contains(player);
}