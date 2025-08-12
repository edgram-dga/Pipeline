using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using MonitorInventario.Models;

public class ApiClient
{
    private readonly HttpClient _client = new();

    public async Task<List<PieceRecord>> GetFromApi1Async(GroupMaterial group, DateTime start, DateTime end)
    {
        var xml = new XElement("Request",
            new XElement("Group", group.Name),
            new XElement("StartDate", start.ToString("yyyy-MM-dd")),
            new XElement("EndDate", end.ToString("yyyy-MM-dd")),
            new XElement("Operations", string.Join(",", group.Operations))
        );

        var response = await _client.PostAsync(AppConfig.Api1Url, new StringContent(xml.ToString(), Encoding.UTF8, "application/xml"));
        response.EnsureSuccessStatusCode();
        // Aquí parsearías JSON o XML real de respuesta
        return new(); // retornar lista de PieceRecord
    }

    public async Task<List<PieceRecord>> GetFromApi2Async(string groupName, DateTime start, DateTime end)
    {
        var xml = new XElement("Request",
            new XElement("Group", groupName),
            new XElement("StartDate", start.ToString("yyyy-MM-dd")),
            new XElement("EndDate", end.ToString("yyyy-MM-dd"))
        );

        var response = await _client.PostAsync(AppConfig.Api2Url, new StringContent(xml.ToString(), Encoding.UTF8, "application/xml"));
        response.EnsureSuccessStatusCode();
        // Aquí parsearías JSON o XML real de respuesta
        return new(); // retornar lista de PieceRecord con Status = DONE
    }
}
