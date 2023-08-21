using App.Common.Interfaces;
using App.Common.Interfaces.Services;
using App.Common.Interfaces.Validators;
using App.Services;
using Infrastructure.Authentication;
using Infrastructure.Common;
using Infrastructure.Configuration;
using Infrastructure.Configuration.ConfigurationManager;
using Infrastructure.Configuration.Provider;
using Infrastructure.DbAccess;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebDiplomaWork.OptionsSetup;
using WebDiplomaWork.Services;
using LocalConfigurationManager = Infrastructure.Configuration.ConfigurationManager.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IConfigurationManager, LocalConfigurationManager>();
builder.Services.AddScoped<IDbAccessProvider, MariaDbAccessProvider>();
builder.Services.Configure<GeneralConfiguration>(
    builder.Configuration.GetSection("GeneralConfiguration"));
builder.Services.AddControllers();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExampleService, ExampleService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.AddScoped<IAccountValidator, AccountValidator>();

builder.Services.AddScoped<IHasher, Hasher>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
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
