using Microsoft.AspNetCore.RateLimiting;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",") ?? new string[] {};

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10); // 10-second window
        opt.PermitLimit = 100; // allow 100 requests per window
    });
});
var configuration = builder.Configuration;

builder.Services.AddSingleton<IRestClient>(
    provider => {
        var options = new RestClientOptions(configuration["RestClientRoot"]);
        var restClient = new RestClient(options);
        // Set default headers that will apply to all requests
        restClient.AddDefaultHeader("Authorization", $"Bearer {configuration["MovieApiKey"]}");
        restClient.AddDefaultHeader("Accept", "application/json");
        return restClient;
    }
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
