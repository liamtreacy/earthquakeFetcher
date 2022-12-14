using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers;

[ApiController]
[Route("[controller]")]
public class RetrieveDataController : ControllerBase
{
    // 
    // GET: /RetrieveData/
    public string Index()
    {
        return "Data will be retrieved...";
    }
}