using Microsoft.AspNetCore.SignalR.Client;

var userGuid = "0e868f0a-d150-4392-abba-c9b98d4d010a"; 

var hubConnection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5201/notificationHub")
    .WithAutomaticReconnect()
    .Build();

hubConnection.On<string>("ReceiveNotification", (message) =>
{
    Console.WriteLine($"Received notification: {message}");
});

await hubConnection.StartAsync();


await hubConnection.SendAsync("JoinGroup", userGuid);

Console.WriteLine("Connected to hub and joined group. Press Ctrl+C to exit.");
Console.CancelKeyPress += async (sender, e) =>
{

    await hubConnection.SendAsync("LeaveGroup", userGuid);
    await hubConnection.DisposeAsync();
};

var tcs = new TaskCompletionSource<object>();
Console.CancelKeyPress += (sender, e) => tcs.SetResult(null);

while (true)
{
    await Task.Delay(100);
}
