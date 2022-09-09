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
        string url = LogsModule.LogsConfig.Webhooks[type];

        if (string.IsNullOrWhiteSpace(url))
            yield break;

        UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        UploadHandlerRaw uploadHandler = new UploadHandlerRaw(JsonSerializer.Serialize(message));
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
            foreach (KeyValuePair<WebhookType, List<string>> webhook in MsgQueue)
            {
                StringBuilder builder = new StringBuilder("");

                foreach (string message in webhook.Value.ToList())
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

                string content = builder.ToString();

                if(string.IsNullOrWhiteSpace(content))
                    continue;

                yield return Timing.WaitUntilDone(Timing.RunCoroutine(SendMessage(new Message(builder.ToString()), webhook.Key)));
            }

            yield return Timing.WaitForSeconds(10);
        }
    }

    private static readonly Dictionary<WebhookType, List<string>> MsgQueue = new() {[WebhookType.CommandLogs] = new List<string>(), [WebhookType.GameLogs] = new List<string>(), [WebhookType.KillLogs] = new List<string>(), [WebhookType.ErrorLogs] = new List<string>(), [WebhookType.ConsoleCommandLogs] = new List<string>()};

    public static string DiscordParse(this string content) => content.Replace("*", "\\*").Replace(":", "\\:").Replace("<", "\\<").Replace(">", "\\>").Replace("_", "\\_").Replace("|", "\\|").Replace("`", "\\`").Replace("~", "\\~");
}