
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    //.WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


string appName = Environment.GetEnvironmentVariable("APP_NAME") ?? "Aplicación no definida";

Console.Clear();
Console.WriteLine($"¡Bienvenido a Comic Manager!");

var services = new ServiceCollection();

//builder de serilog como logger
services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddSerilog(Log.Logger);
});

// Registrar LoggerService, solo una instancia
services.AddSingleton<LoggerService>();

//Registrar servicios
services.AddSingleton<GenreService>();
services.AddSingleton<UserService>();
services.AddSingleton<ComicService>();

// Construir proveedor de servicios
var serviceProvider = services.BuildServiceProvider();


var loggerService = serviceProvider.GetService<LoggerService>();
var genreService = serviceProvider.GetRequiredService<GenreService>();
var userService = serviceProvider.GetRequiredService<UserService>();
var comicService = serviceProvider.GetRequiredService<ComicService>();

loggerService!.LogInformation("Aplicación iniciada");

//Al tener solo 3 servicios, se pasa en el constructor, de lo contrario, se haría a través inyeccción de dependencias como el resto
var menuApp = new MenuApp(
    genreService,
    userService,
    comicService
);
menuApp.ShowMenu();