using SignalRHubService.Hubs;

// задаём подключение до Redis здесь
var redisUrl = "192.168.88.49:6379";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddSignalR()
    // используем Redis backplane
    .AddStackExchangeRedis(redisUrl, options =>
    {
        options.Configuration.ChannelPrefix = "MyRedisApp";
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.Run();