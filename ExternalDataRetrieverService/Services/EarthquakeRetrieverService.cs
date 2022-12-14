using System.Net;

namespace ExternalDataRetrieverService.Services;

public class EarthquakeRetrieverService
{
    private static string GeoFeed =
        "http://earthquake.usgs.gov/earthquakes/feed/v1.0/summary" +
        "/" +
        "all_hour.geojson";
    
    public string Get()
    {
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
}