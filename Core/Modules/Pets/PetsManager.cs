using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using UnityEngine;

namespace Core.Modules.Pets;

public static class PetsManager
{
    public static Dictionary<Player, List<GameObject>> Pets = new();

    public static void SpawnHat(Player player)
    {
        var pets = new List<GameObject>();
        
        var go = new GameObject("Hat").transform;
        go.SetParent(player.GameObject.transform);
        go.localPosition = Vector3.zero;
        pets.Add(go.gameObject);

        var p = Primitive.Create(PrimitiveType.Cylinder, go.position + Vector3.up * 1.19f, null, new Vector3(-0.15f, -0.07f, -0.15f));
        p.Base.gameObject.transform.SetParent(go);
        p.MovementSmoothing = 30;
        p.Color = Color.black;
        p.Spawn();
        
        pets.Add(p.Base.gameObject);

        var p2 = Primitive.Create(PrimitiveType.Cylinder, go.position + Vector3.up * 1.12f, null, new Vector3(-0.2f, -0.02f, -0.2f));
        p2.Base.gameObject.transform.SetParent(go);
        p2.MovementSmoothing = 30;
        p2.Color = Color.black;
        p2.Spawn();
        
        pets.Add(p2.Base.gameObject);

        Pets.Add(player, pets);
    }
}