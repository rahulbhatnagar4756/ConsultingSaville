using Library.Goals.Services;
using Microservice.Base.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Library.Database.Services;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddGoalsServices();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6v9y$B&E)H@McQfTjWnZr4t7w!z%C*F-JaNdRgUkXp2s5v8x/A?D(G+KbPeShVmY")),
            ValidIssuer = Configuration.GetSection("Jwt:Issuer").Value,
            ValidAudience = Configuration.GetSection("Jwt:Audience").Value,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };

    });

builder.Services.AddDatabaseServices();
builder.Services.AddSingleton<ISecureService, SecureService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
