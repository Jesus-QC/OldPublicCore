using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Lobby.Components;

public class JumpTrigger : MonoBehaviour
{
    public void Start()
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = Vector3.one;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Player ply = Player.Get(other.gameObject);
        if (ply is null)
            return;

        ply.Position = Vector3.forward * 300 + Vector3.up * 5;
    }
}