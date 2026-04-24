using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Text;
using Library.Database.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Library.Assess.Services;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAssessServices(); 
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(x =>
    {
        x.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Token failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine("Authentication challenge: " + context.ErrorDescription);
                return Task.CompletedTask;
            }
        };
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:Key").Value)),
            ValidIssuer = Configuration.GetSection("Jwt:Issuer").Value,
            ValidAudience = Configuration.GetSection("Jwt:Audience").Value,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };

    });

builder.Services.AddDatabaseServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
