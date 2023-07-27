using App.Helper;
using App.Repository;
using App.Service;
using Infrastructure.Configuration;
using Infrastructure.Configuration.ConfigurationManager;
using Infrastructure.Configuration.Provider;
using Infrastructure.DbAccess.EfDbContext;
using Infrastructure.DbAccess.Repository;
using Infrastructure.Helper;
using Infrastructure.Service;
using LocalConfigurationManager = Infrastructure.Configuration.ConfigurationManager.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IConfigurationManager, LocalConfigurationManager>();
builder.Services.AddScoped<IDbAccessProvider, MySqlDbAccessProvider>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHasher, Hasher>();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IExampleService, ExampleService>();
builder.Services.AddScoped<IExampleRepository, ExampleRepository>();
builder.Services.AddDbContext<ExampleContext>();

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
