using PrtgAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


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

builder.Services.AddSingleton<IPrtgClientService>(new PrtgClientService("http://127.0.0.1", "ferid", "!BFPz7!dZWc0!3QE"));

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


app.Run();

