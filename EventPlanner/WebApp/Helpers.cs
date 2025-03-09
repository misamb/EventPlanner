namespace WebApp;

public static class Helpers
{
    public static bool IsInFuture(DateTime dateTime)
    {
        return dateTime.CompareTo(DateTime.Now) > 0;
    }
}