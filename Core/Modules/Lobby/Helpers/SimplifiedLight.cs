using AdminToys;
using Mirror;
using UnityEngine;

namespace Core.Modules.Lobby.Helpers;

public class SimplifiedLight
{
    private static LightSourceToy _lightPrefab;
    private static LightSourceToy LightPrefab
    {
        get
        {
            if (_lightPrefab == null)
            {
                foreach (var gameObject in NetworkClient.prefabs.Values)
                    if (gameObject.TryGetComponent<LightSourceToy>(out var component))
                        _lightPrefab = component;
            }

            return _lightPrefab;
        }
    }

    private Vector3 _position;
    private Color _color;
    private float _intensity;
    private bool _castShadows;
    private float _range;

    public SimplifiedLight(Vector3 position, Color color, float intensity, bool shadowCast, float range)
    {
        _position = position;
        _color = color;
        _intensity = intensity;
        _range = range;
        _castShadows = shadowCast;
    }

    public GameObject Spawn(Transform parent)
    {
        var light = Object.Instantiate(LightPrefab, parent);

        light.transform.localPosition = _position;
        light.NetworkLightColor = _color;
        light.NetworkLightShadows = _castShadows;
        light.NetworkLightIntensity = _intensity;
        light.NetworkLightRange = _range;
            
        NetworkServer.Spawn(light.gameObject);

        return light.gameObject;
    }
}