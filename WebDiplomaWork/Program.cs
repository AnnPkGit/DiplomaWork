using WebDiplomaWork.App.DbAccess;
using WebDiplomaWork.Infrastructure.Configuration;
using WebDiplomaWork.Infrastructure.Configuration.ConfigurationManager;
using WebDiplomaWork.Infrastructure.DbAccess.SshAccess;
using WebDiplomaWork.Infrastructure.DbAccess.UserRepository;
using LocalConfigurationManager = WebDiplomaWork.Infrastructure.Configuration.ConfigurationManager.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IConfigurationManager, LocalConfigurationManager>();
builder.Services.AddScoped<ISshConnectionProvider, SshConnectionProvider>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.Configure<GeneralConfiguration>(
    builder.Configuration.GetSection("GeneralConfiguration"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
