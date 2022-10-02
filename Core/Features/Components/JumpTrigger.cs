using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Features.Components;

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
        Player.Get(other.gameObject)?.Kill(DamageType.PocketDimension);
    }
}