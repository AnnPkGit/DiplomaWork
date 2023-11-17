using System.Reflection;
using System.Text.Json.Serialization;
using WebDiplomaWork.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddNewtonsoftJson()
    .AddJsonOptions(x => x.JsonSerializerOptions.MaxDepth = 20)
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpContextAccessor();
    
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables()
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
}

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.UseStaticFiles();
app.MapFallbackToFile("index.html"); 

app.MapHub<NotificationHub>("/sync/notification");
app.Run();
