public static class AppConfig
{
    public static string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=monitor_inventario";

    public static string Api1Url = "https://example.com/api1";
    public static string Api2Url = "https://example.com/api2";

    public static TimeSpan IntervalLast48h = TimeSpan.FromMinutes(30);
    public static TimeSpan IntervalDay2to7 = TimeSpan.FromHours(12);
    public static TimeSpan IntervalDay7to42 = TimeSpan.FromDays(1);
}
