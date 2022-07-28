namespace Core.Features.Data.UI;

public class Notification
{
    public readonly string Message;
    public float Duration = 4f;

    public Notification(string message) => Message = message;
}