using WebDiplomaWork.App;
using WebDiplomaWork.Infrastructure.Configuration;
using WebDiplomaWork.Infrastructure.Configuration.ConfigurationManager;
using WebDiplomaWork.Infrastructure.DbAccess;
using WebDiplomaWork.Infrastructure.DbAccess.SshAccess;
using WebDiplomaWork.Infrastructure.Services;
using WebDiplomaWork.Infrastructure.Services.Helpers;
using LocalConfigurationManager = WebDiplomaWork.Infrastructure.Configuration.ConfigurationManager.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IConfigurationManager, LocalConfigurationManager>();
builder.Services.AddScoped<ISshConnectionProvider, SshConnectionProvider>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHasher, Hasher>();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.Configure<GeneralConfiguration>(
    builder.Configuration.GetSection("GeneralConfiguration"));

var app = builder.Build();

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
