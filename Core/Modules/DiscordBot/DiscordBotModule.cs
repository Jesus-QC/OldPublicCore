using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Loader.Features;
using Exiled.API.Features;

namespace Core.Modules.DiscordBot;

public class DiscordBotModule : CoreModule<DiscordBotConfig>
{
    public override string Name { get; } = "DiscordBot";

    private Task _botTask;
    
    public override void OnEnabled()
    {
        try
        {
            if (!string.IsNullOrEmpty(Config.Path))
            {
                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName is "Core.Bot")
                    {
                        process.Kill();
                    }
                }
                
                File.Copy(Config.Path, Config.Path + ".bak", true);
                
                ProcessStartInfo p = new(Config.Path + ".bak", (Server.Port + 2000).ToString());
                Process.Start(p);
                Log.Warn("Bot started!");
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
        
        _botTask = Task.Run(() => BotTask(Server.Port + 2000, Config.Ip));
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        _botTask.Dispose();
        
        base.OnDisabled();
    }

    private static async Task BotTask(int port, string ip)
    {
        await Task.Delay(35000);
        
        try
        {
            TcpClient c = new ();
            await c.ConnectAsync(ip, port);
            
            while (true)
            {
                await c.GetStream().WriteAsync(new [] {(byte)Player.Dictionary.Count, GetStatus(), (byte)Server.Tps}, 0, 3);
                await Task.Delay(20000);
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }

    private static byte GetStatus()
    {
        try
        {
            if (Round.IsLobby)
                return 3;
        }
        catch
        {
            // ignored
        }

        return 1;
    }
}