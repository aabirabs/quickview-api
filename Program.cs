using PrtgAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text;


var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});

builder.Services.AddSingleton<IPrtgClientService>(new PrtgClientService("http://127.0.0.1", "prtgadmin", "prtgadmin"));
builder.Services.AddSingleton<INotificationHandlerService, NotificationHandlerService>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/devices", (IPrtgClientService prtgClientService) => {
    var client = prtgClientService.GetPrtgClient();
    var devices = client.GetDevices();
    return devices;
});

app.MapGet("/sensors", (IPrtgClientService prtgClientService) => {
    var client = prtgClientService.GetPrtgClient();
    var sensors = client.GetSensors();
    return sensors;
});

// Register an endpoint to receive notifications
app.MapPost("/notification", async (HttpContext context, INotificationHandlerService notificationHandler) =>
{
    using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
    var requestBody = await reader.ReadToEndAsync();

    // Handle the notification
    var result = notificationHandler.HandleNotification(requestBody);

    // Return a response if needed
    await context.Response.WriteAsync(result);
});

app.MapGet("/logs", async (IPrtgClientService prtgClientService) =>
{
    var client = prtgClientService.GetPrtgClient();
    var logs = await client.GetLogsAsync();
    return logs;
});


app.MapGet("/alerts", async (HttpContext context,IPrtgClientService prtgClientService) =>
{
     var client = prtgClientService.GetPrtgClient();
     try
        {
            var sensors = await client.GetSensorsAsync();

            // Filter sensors that are in an alarm state
            var alertSensors = sensors.Where(sensor => { 
                return sensor.Status == Status.Down || sensor.Status == Status.Warning;});


            // Convert sensor data to Alert objects
            var alerts = alertSensors.Select(sensor => new Alert
            {
                Id = sensor.Id,
                Name = sensor.Name,
                Message = $"Sensor '{sensor.Name}' is {sensor.Status}",
                Probe = sensor.Probe,
                Group =sensor.Group,
                Device=sensor.Device,
                DisplayLastValue=sensor.DisplayLastValue
            }).ToList();

            return alerts;
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error fetching alerts: {ex.Message}");
            return new List<Alert>();
        }

});

// New endpoint to get notification actions
app.MapGet("/notificationActions",  (IPrtgClientService prtgClientService) =>
{
    var client = prtgClientService.GetPrtgClient();
    var notificationActions =  client.GetNotificationActions();
    return notificationActions;
});


app.Run();


