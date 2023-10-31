using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace sender.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{

    [HttpPost(Name = "SendNotification")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://neu-api.tech:7001/notificationHub")
            .Build();

        await hubConnection.StartAsync();

        try
        {
            if (request.UserGuid == null)
            {
                await hubConnection.InvokeAsync("SendNotification", request.Message, "ALL");  
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
