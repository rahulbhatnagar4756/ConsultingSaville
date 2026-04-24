using Radzen;
using OneCoreAssessUI.Client.Pages;
using OneCoreAssessUI.Components;
using Serilog;
using Library.Security.Services;
using Library.Database.Services;
using Library.Frog.Services;
using Library.API.Base.Services;  
using OneCoreAssessUI.Services;
using Library.API.Service;
using Library.API.TalentManagement.Services;
using Library.API.Goals.Services;
using Library.API.Admin.Services;
using Library.API.Assess.Services;
using OneCoreAssessUI.Enumerables;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();

builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "OneCoreAssessTheme";
    options.Duration = TimeSpan.FromDays(365);
});

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IFileDownloaderService, FileDownloaderService>(); 

//API Service
builder.Services.AddSingleton<IAPIConnectService, APIConnectService>(); 

//add custom services for libraries
builder.Services.AddDatabaseServices();
builder.Services.AddSecurityServices();
//builder.Services.AddEmployeeSuccessionPlanning();
builder.Services.AddBaseServices();
builder.Services.AddAdminServices();
builder.Services.AddTalentManagementServices();
builder.Services.AddGoalServices();
builder.Services.AddAssessServices();

//for now until the API is running
builder.Services.AddFrogServices();
builder.Services.AddScoped<UserLoggedOnService>();



Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging(); 
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
 
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(o => o.ContentSecurityFrameAncestorsPolicy = "'self' *.savilleconsulting.co.za")
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(OneCoreAssessUI.Client._Imports).Assembly);

app.Run();
