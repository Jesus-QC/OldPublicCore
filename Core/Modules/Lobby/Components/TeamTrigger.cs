using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Lobby.Components;

public class TeamTrigger : MonoBehaviour
{
    public Team team;

    private readonly List<Player> _players = new();

    public void Start()
    {
        CapsuleCollider col = gameObject.AddComponent<CapsuleCollider>();
        col.isTrigger = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Player ply = Player.Get(other.gameObject);
        if (ply is null || _players.Contains(ply))
            return;

        _players.Add(ply);
    }

    private void OnTriggerExit(Collider other)
    {
        Player ply = Player.Get(other.gameObject);
            
        if (ply is null || !_players.Contains(ply))
            return;

        _players.Remove(ply);
    }

    public bool ContainsPlayer(Player player) => _players.Contains(player);
}