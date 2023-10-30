using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace sender.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5201/notificationHub")
            .Build();

        await hubConnection.StartAsync();

        try
        {
            if (request.UserGuid == null)
            {
                await hubConnection.InvokeAsync("SendNotification", request.Message);  
            }
            else
            {
                await hubConnection.InvokeAsync("SendNotification", request.Message, request.UserGuid);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        finally
        {
            await hubConnection.StopAsync();
        }
    }

}

public class NotificationRequest
{
    public Guid? UserGuid { get; set; }
    public string Message { get; set; }
}
