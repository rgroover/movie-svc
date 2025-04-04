using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using movie_svc.Services;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IFavoritesService, FavoritesService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        NameClaimType = ClaimTypes.NameIdentifier
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            Console.WriteLine("Exception type: " + context.Exception.GetType().Name);
            if (context.Exception.InnerException != null)
            {
                Console.WriteLine("Inner exception: " + context.Exception.InnerException.Message);
            }
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            var auth = context.Request.Headers.Authorization.ToString();
            Console.WriteLine($"Authorization header: {auth}");
            
            if (!string.IsNullOrEmpty(auth) && auth.StartsWith("Bearer "))
            {
                var token = auth.Substring("Bearer ".Length).Trim();
                Console.WriteLine($"Token starts with: {token.Substring(0, Math.Min(20, token.Length))}...");
                Console.WriteLine($"Token length: {token.Length}");
                Console.WriteLine($"Token contains dots: {token.Contains('.')}");
                Console.WriteLine($"Number of dots: {token.Count(c => c == '.')}");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
        
            if (claimsIdentity != null)
            {
                // First, print all claims to see what we're getting
                foreach (var claim in claimsIdentity.Claims)
                {
                    System.Diagnostics.Debug.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                }
            
                // Look for email in Auth0's custom claim
                var emailClaim = claimsIdentity.FindFirst("email");
                if (emailClaim != null)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, emailClaim.Value));
                }
            
                // Look for name in Auth0's custom claim
                var nameClaim = claimsIdentity.FindFirst("name");
                if (nameClaim != null)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, nameClaim.Value));
                }
            }
        
            return Task.CompletedTask;
        }
    };
});

// Add authorization
builder.Services.AddAuthorization();

// Add API Explorer for Swagger
builder.Services.AddEndpointsApiExplorer();

// Add Swagger with Auth0 support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Your API", Version = "v1" });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


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

builder.Services.AddHttpClient<IRestClientService, RestClientService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(sp =>
{
    var endpoint = builder.Configuration["CosmosDb:EndpointUri"];
    var key = builder.Configuration["CosmosDb:PrimaryKey"];
    var options = new CosmosClientOptions
    {
        ConnectionMode = ConnectionMode.Gateway, // or Direct for better performance
        UseSystemTextJsonSerializerWithOptions = new System.Text.Json.JsonSerializerOptions()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        }
    };

    return new CosmosClient(endpoint, key, options);
});

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

// 2. Enable authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
