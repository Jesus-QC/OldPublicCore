namespace Core.Features.Logger;

public static class LogUtils
{
    public static string GetColor(LogColor color) => GetColor((byte)color);
    public static string GetColor(byte ansiCode) => $"\u001b[38;5;{ansiCode}m";
    
    public static string GetBackgroundColor(LogColor color) => GetBackgroundColor((byte)color);
    public static string GetBackgroundColor(byte ansiCode) => $"\u001b[48;5;{ansiCode}m";
}