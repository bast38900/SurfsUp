using Microsoft.AspNetCore.Mvc;
using SurfsUpLibrary.WeatherDTO;
using System.Text.Json;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        static string cityName = "Odense";
        static string openweatherBaseURI = "https://api.openweathermap.org";
        static string openweatherURI = $"/data/2.5/forecast?q={cityName}&appid=";
        static string openweatherAPIkey = "808f482a7577345beabc44b2feaf1db6";

        [HttpGet]
        public async Task<ActionResult<Root>> Index()
        {
            using HttpClient client = new() { BaseAddress = new Uri(openweatherBaseURI) };
            using HttpResponseMessage response = await client.GetAsync(openweatherURI + openweatherAPIkey);
            response.EnsureSuccessStatusCode();
            Root? root = JsonSerializer.Deserialize<Root>(await response.Content.ReadAsStringAsync());
            return Ok(root);
        }
    }
}