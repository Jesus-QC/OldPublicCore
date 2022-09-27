using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace Core.Features.Handlers;

public static class PollHandler
{
    public static bool Enabled = false;
    public static string PollAuthor = string.Empty;
    public static string PollName = string.Empty;
    public static int YesVotes;
    public static int NoVotes;
    public static int TimeLeft;
    public static readonly HashSet<string> Voted = new();

    public static void AddPoll(string author, string description, int time, Action todo)
    {
        if(Enabled)
            return;
        
        Voted.Clear();
        PollAuthor = author;
        PollName = description;
        YesVotes = 0;
        NoVotes = 0;
        TimeLeft = time;
        Enabled = true;
        
        Task.Run(async () =>
        {
            Log.Info(TimeLeft);
            while (TimeLeft >= 0)
            {
                await Task.Delay(1000);
                TimeLeft--;
                Log.Info(TimeLeft);
            }

            Enabled = false;

            if (YesVotes > NoVotes)
                todo();
        });
    }
    
    
}