using Library.Base.Services;
using Library.Database.Services;
using Library.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(x =>
    {
        //x.Events = new JwtBearerEvents
        //{
        //    OnAuthenticationFailed = context =>
        //    {
        //        Console.WriteLine("Token failed: " + context.Exception.Message);
        //        return Task.CompletedTask;
        //    },
        //    OnChallenge = context =>
        //    {
        //        Console.WriteLine("Authentication challenge: " + context.ErrorDescription);
        //        return Task.CompletedTask;
        //    }
        //};

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
builder.Services.AddAdminServices();
builder.Services.AddEmployeeServices(); 
builder.Services.AddUsersServices(); 
builder.Services.AddCompanyServices(); 


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
