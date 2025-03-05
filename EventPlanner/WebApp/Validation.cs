namespace WebApp;

public static class Validation
{
    public static bool IsInFuture(DateTime dateTime)
    {
        return dateTime.CompareTo(DateTime.Now) > 0;
    }
}