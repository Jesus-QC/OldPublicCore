using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Features.Data.Enums;
using Core.Features.Data.UI;
using Core.Features.Extensions;
using Exiled.API.Features;
using Hints;
using UnityEngine;

namespace Core.Features.Components;

public class CustomHUD : MonoBehaviour
{
    private static readonly string DefaultHUD = $"<line-height=95%><voffset=8em><size=50%><alpha=#44><align=left>W<lowercase>olf</lowercase>P<lowercase>ack</lowercase> (Ver {Core.Instance.Version}) | [9]</align><alpha=#FF></size>\n<align=right>[0]</align>[1][2][3][4]";
    private static readonly Dictionary<int, int> MessageLines = new() { [0] = 7, [1] = 6, [2] = 6, [3] = 5, [4] = 3 };
        
    private readonly Dictionary<int, float> _timers = new() { [0] = -1, [1] = -1, [2] = -1, [3] = -1, [4] = -1 };
    private readonly Dictionary<int, string> _messages = new() { [0] = string.Empty, [1] = string.Empty, [2] = string.Empty, [3] = string.Empty, [4] = string.Empty };

    private float _counter;

    private Player _player;

    private void Start() => _player = Player.Get(gameObject);

    private void Update()
    {
        if(!Round.IsStarted)
            return;
            
        _counter += Time.deltaTime;

        if (_counter > .99f)
        {
            UpdateLevels();
            var msg = GetMessage();
            _player.Connection.Send(new HintMessage(new TextHint(msg, new HintParameter[]{ new StringHintParameter(msg) }, null, 1.2f)));
            _counter = 0;
        }
    }

    public void AddMessage(ScreenZone zone, string message, float time = 10f)
    {
        var i = (int) zone;
        _messages[i] = message;
        _timers[i] = time;
    }

    public void ClearZone(ScreenZone zone)
    {
        var i = (int) zone;
        _messages[i] = string.Empty;
        _timers[i] = -1;
    }

    private string GetMessage()
    {
        var builder = new StringBuilder(DefaultHUD);

        builder = builder.Replace("[9]", $"{_player.Nickname} ({_player.Id})");
        builder = builder.Replace("[0]", FormatStringForHud(_messages[0], MessageLines[0]));
            
        for (var i = 1; i < _timers.Count; i++)
        {
            if (_timers[i] >= 0)
                _timers[i]--;

            if (_timers[i] < 0)
                _messages[i] = string.Empty;

            var message = _messages[i].TrimEnd('\n');
                
            if (string.IsNullOrEmpty(message))
                message = '\n' + message;
                
            builder = builder.Replace($"[{i}]", FormatStringForHud(message, MessageLines[i]));
        }

        return builder.ToString();
    }
        
    private static string FormatStringForHud(string text, int linesNeeded)
    {
        var builder = new StringBuilder();
        builder.Append(text);
           
        var textLines = text.Count(x => x == '\n');

        for (var i = 0; i < linesNeeded - textLines; i++)
            builder.Append('\n');

        return builder.ToString();
    }

    private readonly Dictionary<Perk, PerkMessage> _levelMessages = new();

    public void AddPerkMessage(Perk perk, PerkMessage perkMessage)
    {
        if (_levelMessages.ContainsKey(perk))
        {
            _levelMessages[perk].ExpAmount += perkMessage.ExpAmount;
            _levelMessages[perk].DurationLeft += perkMessage.DurationLeft;
            return;
        }

        _levelMessages.Add(perk, perkMessage);
    }
        
    private void UpdateLevels()
    {
        if (_player.DoNotTrack)
            return;

        if (_player.Role == RoleType.Spectator)
        {
            _messages[0] = $"<color=#9effe0>spectators:</color> {Player.Get(RoleType.Spectator).Count()}\n<color=#9ecfff>mtf tickets:</color> {Respawn.NtfTickets}\n<color=#9effa6>chaos tickets:</color> {Respawn.ChaosTickets}";
            return;
        }

        var builder = new StringBuilder($"<color=#505050>level:</color> {_player.GetLevel()}\n");

        var i = 0;

        foreach (var perk in _levelMessages.Keys.ToList())
        {
            if (i == 6)
                break;

            var perky = _levelMessages[perk];
            perky.DurationLeft -= 1;

            if (perky.DurationLeft < 0)
            {
                _levelMessages.Remove(perk);
                continue;
            }

            builder.Append($"[<color=#{perky.Color}>{perky.Message}</color> | <color=#ffff57>+ {perky.ExpAmount}</color>]\n");
            i++;
        }

        _messages[0] = builder.ToString().TrimEnd('\n');
    }
}