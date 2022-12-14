using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using ExternalDataRetrieverService.Services;

namespace MvcMovie.Controllers;

[ApiController]
[Route("[controller]")]
public class RetrieveDataController : ControllerBase
{
    // 
    // GET: /RetrieveData/
    public string Index()
    {
        var service = new EarthquakeRetrieverService();
        var str = service.Get();
        return "Data will be retrieved... " + str;
    }
}