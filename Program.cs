using MonitorInventario.Models;

var db = new DatabaseHelper();
var api = new ApiClient();

var grupos = await db.GetGroupsAsync();

foreach (var g in grupos)
{
    foreach (var range in new[] {
        new {Start = DateTime.UtcNow.AddDays(-2), End = DateTime.UtcNow, Tag="48h", Interval=AppConfig.IntervalLast48h},
        new {Start = DateTime.UtcNow.AddDays(-7), End = DateTime.UtcNow.AddDays(-2), Tag="2-7d", Interval=AppConfig.IntervalDay2to7},
        new {Start = DateTime.UtcNow.AddDays(-42), End = DateTime.UtcNow.AddDays(-7), Tag="7-42d", Interval=AppConfig.IntervalDay7to42}
    })
    {
        var last = await db.GetLastRequestAsync(g.Id, range.Tag);
        if (last != null && DateTime.UtcNow - last < range.Interval)
            continue;

        var api1Data = await api.GetFromApi1Async(g, range.Start, range.End);
        var api2Data = await api.GetFromApi2Async(g.Name, range.Start, range.End);

        foreach (var p in api1Data)
            await db.UpsertPieceAsync(p);

        foreach (var p in api2Data)
        {
            p.Status = "DONE";
            await db.UpsertPieceAsync(p);
        }

        await db.UpdateLastRequestAsync(g.Id, range.Tag);
    }
}
