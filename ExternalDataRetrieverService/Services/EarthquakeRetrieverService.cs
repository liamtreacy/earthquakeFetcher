using System.Net;
using ExternalDataRetrieverService.Models;
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

    // change to List<Earthquake>
    string ParseEarthquakes(string jsonStr)
    {
        dynamic json  = JsonConvert.DeserializeObject(jsonStr);

        dynamic features = json["features"];
        return features[0]["properties"]["place"];
    }
    
    public string Get()
    {
        var jsonStr = PerformGetRequest();

        var eqs = ParseEarthquakes(jsonStr);
        return eqs;
    }
}