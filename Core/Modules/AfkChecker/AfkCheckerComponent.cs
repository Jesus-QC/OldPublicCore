using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.AfkChecker;

public class AfkCheckerComponent : MonoBehaviour
{
    private float _counter;
    private int _afkTime;
    private Vector3 _lastPos;
    private Vector2 _lastRot;

    public Player Player;

    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter < 1)
            return;
        
        Vector3 pos = Player.Position; 
        Vector2 rot = Player.Rotation;
            
        if (Player.Role != RoleType.Spectator && Player.Role != RoleType.Tutorial && _lastPos == pos && _lastRot == rot)
        {
            _afkTime++;
                
            if (_afkTime >= AfkCheckerModule.StaticConfig.AfkTime - 10)
            {
                Player.Broadcast(1, $"<b><color=#ff4940>You were detected as afk.</color>\nMove in less than {AfkCheckerModule.StaticConfig.AfkTime - _afkTime} seconds or you will be kicked.</b>");

                if (_afkTime == AfkCheckerModule.StaticConfig.AfkTime)
                {
                    if (PermissionsHandler.IsPermitted(Player.Group.Permissions, PlayerPermissions.AFKImmunity))
                    {
                        Destroy(this);
                        return;
                    }
                        
                    Player.SetRole(RoleType.Spectator);
                    _lastPos = pos;
                    _lastRot = rot;
                    _afkTime = 0;
                }
            }
        }
        else
        {
            _lastPos = pos;
            _lastRot = rot;
            _afkTime = 0;
        }

        _counter = 0;
        
    }
}