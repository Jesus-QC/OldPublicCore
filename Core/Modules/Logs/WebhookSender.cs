using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Modules.Logs.Enums;
using Core.Modules.Logs.Webhook;
using Exiled.API.Features;
using MEC;
using UnityEngine.Networking;
using Utf8Json;

namespace Core.Modules.Logs;

public static class WebhookSender
{
    public static void AddMessage(string content, WebhookType type)
    {
        content = $"<t:{((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds()}:T> " + content;
        MsgQueue[type].Add(content);
    }
        
    private static IEnumerator<float> SendMessage(Message message, WebhookType type)
    {
        var url = LogsModule.LogsConfig.Webhooks[type];

        if (string.IsNullOrWhiteSpace(url))
            yield break;

        var webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        var uploadHandler = new UploadHandlerRaw(JsonSerializer.Serialize(message));
        uploadHandler.contentType = "application/json";
        webRequest.uploadHandler = uploadHandler;

        yield return Timing.WaitUntilDone(webRequest.SendWebRequest());

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Log.Error($"Error sending the message: {webRequest.responseCode} - {webRequest.error}");
        }
    }

    public static IEnumerator<float> ManageQueue()
    {
        while (true)
        {
            foreach (var webhook in MsgQueue)
            {
                var builder = new StringBuilder("");

                foreach (var message in webhook.Value.ToList())
                {
                    if (builder.Length + message.Length < 2000)
                    {
                        builder.AppendLine(message);
                        MsgQueue[webhook.Key].Remove(message);
                    }
                    else
                    {
                        break;
                    }
                }

                var content = builder.ToString();

                if(string.IsNullOrWhiteSpace(content))
                    continue;

                yield return Timing.WaitUntilDone(Timing.RunCoroutine(SendMessage(new Message(builder.ToString()), webhook.Key)));
            }

            yield return Timing.WaitForSeconds(10);
        }
    }

    private static readonly Dictionary<WebhookType, List<string>> MsgQueue = new() {[WebhookType.CommandLogs] = new List<string>(), [WebhookType.GameLogs] = new List<string>(), [WebhookType.KillLogs] = new List<string>(), [WebhookType.ErrorLogs] = new List<string>()};

    public static string DiscordParse(this string content) => content.Replace("*", "\\*").Replace(":", "\\:").Replace("<", "\\<").Replace(">", "\\>").Replace("_", "\\_").Replace("|", "\\|").Replace("`", "\\`").Replace("~", "\\~");
}