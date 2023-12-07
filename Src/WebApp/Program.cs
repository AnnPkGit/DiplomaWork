using System.Reflection;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;
using WebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.MaxDepth = 20;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddHttpContextAccessor();
    
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables()
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
}
if (app.Environment.IsProduction())
{
    var keyVaultName = builder.Configuration["KeyVaultName"];
    var kvUri = $"https://{keyVaultName}.vault.azure.net";
    var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
    builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); 

app.MapHub<NotificationHub>("/sync/notification");
app.Run();
