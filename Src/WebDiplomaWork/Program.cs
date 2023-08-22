using LocalConfigurationManager = Infrastructure.Configuration.ConfigurationManager.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
    
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // Добавление маршрута для контроллеров api
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});
app.UseStaticFiles();
app.MapFallbackToFile("index.html"); 
app.Run();
