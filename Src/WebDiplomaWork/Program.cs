using App.Common;
using App.Repository;
using App.Service;
using App.Validators;
using Infrastructure.Authentication;
using Infrastructure.Configuration;
using Infrastructure.Configuration.ConfigurationManager;
using Infrastructure.Configuration.Provider;
using Infrastructure.DbAccess.EfDbContext;
using Infrastructure.DbAccess.Repository;
using Infrastructure.Helper;
using Infrastructure.Service;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebDiplomaWork.Controller;
using WebDiplomaWork.OptionsSetup;
using LocalConfigurationManager = Infrastructure.Configuration.ConfigurationManager.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IConfigurationManager, LocalConfigurationManager>();
builder.Services.AddScoped<IDbAccessProvider, MariaDbAccessProvider>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHasher, Hasher>();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IExampleService, ExampleService>();
builder.Services.AddScoped<IExampleRepository, ExampleRepository>();
builder.Services.AddDbContext<ExampleContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.Configure<GeneralConfiguration>(
    builder.Configuration.GetSection("GeneralConfiguration"));
    builder.Services.AddControllers();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
    
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
