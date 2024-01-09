namespace Corvid.Api.Helpers;

public static class DateOnlyHelper
{
    /// <summary>
    /// Initialize a static instance of the <see cref="DateOnlyHelper"/> class
    /// </summary>
    static DateOnlyHelper()
        => TodayFunc = () => DateOnly.FromDateTime(DateTime.Today);
    
    public static DateOnly Today => TodayFunc();
    
    private static Func<DateOnly> TodayFunc { get; set; }

    public static void Set(DateOnly date) 
        => TodayFunc = () => date;

    public static void Reset()
        => TodayFunc = () => DateOnly.FromDateTime(DateTime.Today);
}