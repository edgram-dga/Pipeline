namespace MonitorInventario.Models;

public class PieceRecord
{
    public string IdPieza { get; set; } = "";
    public string ItemNumber { get; set; } = "";
    public string ItemType { get; set; } = "";
    public string ItemVersion { get; set; } = "";
    public string ItemDescription { get; set; } = "";
    public DateTime Date { get; set; }
    public string Operation { get; set; } = "";
    public string Status { get; set; } = ""; // NORMAL o DONE
}
