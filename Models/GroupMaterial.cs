namespace MonitorInventario.Models;

public class GroupMaterial
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<string> Operations { get; set; } = new();
}
