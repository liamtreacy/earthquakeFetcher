namespace ExternalDataRetrieverService.Models;

public class Earthquake
{
    public string Id{ get; set; }
    public string Place { get; set; }
    public Decimal Magnitude { get; set; }

    public DateTime Time { get; set; }
}