namespace ExternalDataRetrieverService.Models;

public class Earthquake
{
#pragma warning disable CS8618
    public string Id { get; set; }
    public string Place { get; set; }
#pragma warning restore CS8618
    public Decimal Magnitude { get; set; }

    public DateTime Time { get; set; }
}