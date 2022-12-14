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
        // TODO:: Improve this
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GeoFeed);
        request.AutomaticDecompression = 
            DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using(Stream stream = response.GetResponseStream())
        using(StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
    
    List<Earthquake> ParseEarthquakes(string jsonStr)
    {
        dynamic json  = JsonConvert.DeserializeObject(jsonStr);

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
        var places = "";

        foreach (var earthquake in eqs)
        {
            context.Earthquakes.Add(earthquake);

            await context.SaveChangesAsync();
        }

        return true;
    }
    
}