using System.Net;
using EarthquakeRetrieverService.Models;
using ExternalDataRetrieverService.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExternalDataRetrieverService.Services;

public class EarthquakeRetrieverService
{
    private static string GeoFeed =
        "http://earthquake.usgs.gov/earthquakes/feed/v1.0/summary" +
        "/" +
        "all_hour.geojson";

    private string PerformGetRequest()
    {
        HttpClient client = new HttpClient();
        var response = client.GetStringAsync(GeoFeed).Result;
        return response;
    }
    
    List<Earthquake>? ParseEarthquakes(string jsonStr)
    {
        dynamic json  = JsonConvert.DeserializeObject(jsonStr) ?? throw new InvalidOperationException();

        if (json == null)
            return null;

        dynamic features = json["features"];

        var erqs = new List<Earthquake>();

        foreach (var erq in features)
        {
            erqs.Add(new Earthquake
            {
                Id = erq["id"],
                Magnitude = erq["properties"]["mag"],
                Place = erq["properties"]["place"],
                Time = new DateTime((long)(erq["properties"]["time"]))
            });
        }

        return erqs;
    }
    
    public async Task<bool> Get(EarthquakeContext context)
    {
        var jsonStr = PerformGetRequest();
        var eqs = ParseEarthquakes(jsonStr);

        if (eqs == null)
            return false;

        foreach (var earthquake in eqs)
        {
            if (EarthquakeExists(context, earthquake.Id))
                break;
            
            context.Earthquakes.Add(earthquake);

            await context.SaveChangesAsync();
        }

        return true;
    }
    
    private bool EarthquakeExists(EarthquakeContext context, string id)
    {
        return (context.Earthquakes?.Any(e => e.Id == id)).GetValueOrDefault();
    }
    
}