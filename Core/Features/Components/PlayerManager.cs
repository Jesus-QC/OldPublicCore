using System;
using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Features.Components;

public class PlayerManager : MonoBehaviour
{
    private DateTime _startTime;
    private CustomHUD _playerHUD;

    private void Awake() => _playerHUD = gameObject.GetComponent<CustomHUD>();
    private void Start() => _startTime = DateTime.Now;

    public void SendHint(ScreenZone z, string msg, float duration) => _playerHUD.AddMessage(z, msg, duration);
    public void ClearHint(ScreenZone z) => _playerHUD.ClearZone(z);
    public int GetSeconds => (int)(DateTime.Now - _startTime).TotalSeconds;
}