string appName = Environment.GetEnvironmentVariable("APP_NAME") ?? "Aplicación no definida";

Console.WriteLine($"¡Bienvenido a {appName}!");

var menuApp = new MenuApp();
menuApp.ShowMenu();