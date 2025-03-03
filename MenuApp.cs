using System.Net.Http.Headers;
using Models;
using Services;
using Spectre.Console;


class MenuApp
{

    private bool exit = false;
    private bool back = false;

    public MenuApp()
    {
        AnsiConsole.Write(
            new FigletText("Comic Manager")
            .LeftJustified()
            .Color(Color.Red));
    }

    public void ShowMenu()
    {
        
        //int option = 0;
        var option = "";
        
        do
        {

            var options = GetOptionsMenu();

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold cyan]--- MENÚ PRINCIPAL ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();
            
            /*AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold cyan]--- MENÚ PRINCIPAL ---[/]");
            AnsiConsole.WriteLine("1. Géneros");
            AnsiConsole.WriteLine("2. Comics");
            
            if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
            {
                AnsiConsole.WriteLine("3. Usuarios");
            }

            if (UserService.currentUser != null)
            {
                AnsiConsole.WriteLine("4. Zona privada");
                AnsiConsole.WriteLine("5. Cerrar sesión");
                AnsiConsole.WriteLine("6. Salir");
            }
            else
            {
                AnsiConsole.WriteLine("4. Iniciar sesión");
                AnsiConsole.WriteLine("5. Registrarse");
                AnsiConsole.WriteLine("6. Salir");
            }

            option = AnsiConsole.Prompt(
                new TextPrompt<int>("Selecciona una opción: ")
                    .PromptStyle("yellow")
                    .Validate(x => x >= 1 && x <= 6 ? ValidationResult.Success() : ValidationResult.Error("Opción inválida, selecciona entre 1 y 6."))); */

            /*switch (option)
            {
                case 1:
                    ShowGenreMenu();
                    break;
                case 2:
                    ShowComicMenu();
                    break;
                case 3:
                    ShowUserMenu();
                    break;
                case 4:
                    if (UserService.currentUser != null)
                        ShowPrivateMenu();
                    else
                        UserService.Login();
                    break;
                case 5:
                    if (UserService.currentUser != null)
                        UserService.Logout();
                    else
                        UserService.AddUser();
                    break;
                case 6:
                    AnsiConsole.MarkupLine("[bold green]¡Hasta pronto![/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]La opción no es correcta[/]");
                    break;
            }*/

            /*AnsiConsole.MarkupLine("[green]Presiona una tecla para continuar...[/]");
            Console.ReadKey();*/
        }

        while(!exit);

        AnsiConsole.MarkupLine("[bold green]¡Hasta pronto![/]");
    }




    private void ShowGenreMenu()
    {
        string option = "";

        do
        {

            var options = GetOptionsGenreMenu();

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold cyan]--- MENÚ GÉNEROS ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();

            /*AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]--- MENÚ GÉNEROS ---[/]");

            if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
            {
                AnsiConsole.WriteLine("1. Añadir género");
            }
            AnsiConsole.WriteLine("2. Listar géneros");
            AnsiConsole.WriteLine("3. Buscar género");

            if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
            {
                AnsiConsole.WriteLine("4. Eliminar género");
            }

            AnsiConsole.WriteLine("5. Volver al menú principal");

            option = AnsiConsole.Prompt(
                new TextPrompt<int>("Selecciona una opción: ")
                    .PromptStyle("yellow")
                    .Validate(x => x >= 1 && x <= 5 ? ValidationResult.Success() : ValidationResult.Error("Opción inválida, selecciona entre 1 y 5.")));

                    AnsiConsole.WriteLine($"Opción seleccionada: {option}");

            switch (option)
            {
                case 1:
                    if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
                        GenreService.AddGenre();
                    else
                        AnsiConsole.MarkupLine("[red]Error: Por favor selecciona una opción válida[/]");
                    break;
                case 2:
                    GenreService.ShowAllGenres();
                    break;
                case 3:
                    GenreService.SearchGenre();
                    break;
                case 4:
                    if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
                        GenreService.DeleteGenre();
                    else
                        AnsiConsole.MarkupLine("[red]Error: Por favor selecciona una opción válida[/]");
                    break;
                case 5:
                    AnsiConsole.MarkupLine("[blue]Volviendo..[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]La opción no es correcta[/]");
                    break;
            }*/

            //AnsiConsole.MarkupLine("[green]Presiona una tecla para continuar...[/]");
            //Console.ReadKey();

        }
        while (!back);

    }



    private void ShowComicMenu()
    {
        string option = "";

        do
        {

            var options = GetOptionsComicMenu();

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold cyan]--- MENÚ CÓMICS ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();
            /*AnsiConsole.MarkupLine("[bold yellow]--- MENÚ CÓMICS ---[/]");
            AnsiConsole.WriteLine("1. Añadir cómic");
            AnsiConsole.WriteLine("2. Listar cómics");
            AnsiConsole.WriteLine("3. Buscar cómic");
            AnsiConsole.WriteLine("4. Eliminar cómic");
            AnsiConsole.WriteLine("5. Volver al menú principal");

            option = AnsiConsole.Prompt(
                new TextPrompt<int>("Selecciona una opción: ")
                    .PromptStyle("yellow")
                    .Validate(x => x >= 1 && x <= 5 ? ValidationResult.Success() : ValidationResult.Error("Opción inválida, selecciona entre 1 y 5.")));

            switch (option)
            {
                case 1:
                    if (UserService.currentUser != null)
                        ComicService.AddComic();
                    else
                        AnsiConsole.MarkupLine("[red]Debes iniciar sesión para acceder a esta opción[/]");
                    break;
                case 2:
                    ComicService.ShowAllComics();
                    break;
                case 3:
                    ComicService.SearchComic();
                    break;
                case 4:
                    if (UserService.currentUser != null)
                        ComicService.DeleteComic();
                    else
                        AnsiConsole.MarkupLine("[red]Debes iniciar sesión para acceder a esta opción[/]");
                    break;
                case 5:
                    AnsiConsole.MarkupLine("[blue]Volviendo..[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]La opción no es correcta[/]");
                    break;
            }

            AnsiConsole.MarkupLine("[green]Presiona una tecla para continuar...[/]");
            Console.ReadKey();*/

        }
        while (!back);

    }




    private void ShowUserMenu()
    {
        string option = "";

        do
        {
            var options = GetOptionsUserMenu();

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold cyan]--- MENÚ USUARIOS ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();
            /*
            AnsiConsole.MarkupLine("[bold yellow]--- MENÚ DE USUARIOS ---[/]");
            AnsiConsole.WriteLine("1. Añadir usuario");
            AnsiConsole.WriteLine("2. Listar usuarios");
            AnsiConsole.WriteLine("3. Buscar usuario");
            AnsiConsole.WriteLine("4. Eliminar usuario");
            AnsiConsole.WriteLine("5. Volver al menú principal");

            option = AnsiConsole.Prompt(
                new TextPrompt<int>("Selecciona una opción: ")
                    .PromptStyle("yellow")
                    .Validate(x => x >= 1 && x <= 5 ? ValidationResult.Success() : ValidationResult.Error("Opción inválida, selecciona entre 1 y 5.")));

            switch (option)
            {
                case 1:
                    UserService.AddUser();
                    break;
                case 2:
                    UserService.ShowAllUsers();
                    break;
                case 3:
                    UserService.SearchUser();
                    break;
                case 4:
                    UserService.DeleteUser();
                    break;
                case 5:
                    AnsiConsole.MarkupLine("[blue]Volviendo..[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]La opción no es correcta[/]");
                    break;
            }

        */
        
        }
        while (!back);

    }



    private void ShowPrivateMenu()
    {
        string option = "";

        do
        {
            var options = GetOptionsPrivateMenu();

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold cyan]--- ZONA PRIVADO ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();
            /*AnsiConsole.MarkupLine("[bold yellow]--- ZONA PRIVADA ---[/]");
            AnsiConsole.WriteLine("1. Añadir cómic a mi lista personal");
            AnsiConsole.WriteLine("2. Eliminar cómic a mi lista personal");
            AnsiConsole.WriteLine("3. Ver mi lista personal de cómics");
            AnsiConsole.WriteLine("4. Ver mis datos personales ");
            AnsiConsole.WriteLine("5. Volver al menú principal");

            option = AnsiConsole.Prompt(
                new TextPrompt<int>("Selecciona una opción: ")
                    .PromptStyle("yellow")
                    .Validate(x => x >= 1 && x <= 5 ? ValidationResult.Success() : ValidationResult.Error("Opción inválida, selecciona entre 1 y 5.")));

            switch (option)
            {
                case 1:
                    UserService.ManageComicsInUserList(UserService.currentUser!, true);
                    break;
                case 2:
                    UserService.ManageComicsInUserList(UserService.currentUser!, false);
                    break;
                case 3:
                    UserService.ShowUserComics(UserService.currentUser!);
                    break;
                case 4:
                    UserService.ViewUserData();
                    break;
                case 5:
                    AnsiConsole.MarkupLine("[blue]Volviendo..[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]La opción no es correcta[/]");
                    break;
            }*/

        } while (!back);
    }

    private Dictionary<string, Action> GetOptionsMenu()
    {
        var options = new Dictionary<string, Action>

        {
            { "Géneros", ShowGenreMenu },
            { "Cómics", ShowComicMenu },
        };

        if (UserService.currentUser == null)
        {
            options["Iniciar Sesión"] = UserService.Login;
            options["Registrarse"] = UserService.AddUser;
        }
        else
        {
            options["Zona Privada"] = ShowPrivateMenu;
            options["Cerrar Sesión"] = UserService.Logout;

            if (UserService.currentUser.IsAdmin)
            {
                options["Usuarios"] = ShowUserMenu;
            }
        }

        {
            options["Salir"] = ExitMenu;
        }
            
        return options;
    }


    private Dictionary<string, Action> GetOptionsGenreMenu()
    {
        var options = new Dictionary<string, Action>

        {
            { "Listar géneros", GenreService.ShowAllGenres },
            { "Buscar género", GenreService.SearchGenre },
        };

        if (UserService.currentUser?.IsAdmin == true)
        {
            options["Añadir género"] = GenreService.AddGenre;
            options["Eliminar género"] = GenreService.DeleteGenre;
        }

        {
            options["Volver al menú principal"] = BackMenu;
        }
            
        return options;
    }


    private Dictionary<string, Action> GetOptionsComicMenu()
    {
        var options = new Dictionary<string, Action>

        {
            { "Listar cómics", ComicService.ShowAllComics },
            { "Buscar cómic", ComicService.SearchComic},
        };

        if (UserService.currentUser != null)
        {
            options["Añadir cómic"] = ComicService.AddComic;

            if (UserService.currentUser.IsAdmin) 
            {
                options["Eliminar cómic"] = ComicService.DeleteComic;
            }
        }
            
        {
            options["Volver al menú principal"] = BackMenu;
        }
            
        return options;
    }


    private Dictionary<string, Action> GetOptionsUserMenu()
    {
        var options = new Dictionary<string, Action>

        {
            { "Añadir usuario", UserService.AddUser },
            { "Listar usuarios", UserService.ShowAllUsers },
            { "Buscar usuario", UserService.SearchUser },
            { "Eliminar usuario", UserService.DeleteUser},
            { "Volver al menú principal", BackMenu}
        };
            
        return options;
    }


    private Dictionary<string, Action> GetOptionsPrivateMenu()
    {
        var options = new Dictionary<string, Action>

        {
            { "Añadir cómic a la lista personal", () => UserService.ManageComicsInUserList(UserService.currentUser!, true) },
            { "Eliminar cómic de la lista personal", () => UserService.ManageComicsInUserList(UserService.currentUser!, false) },
            { "Ver mi lista personal de cómics",() => UserService.ShowUserComics(UserService.currentUser!)},
            { "Ver mis datos personales", UserService.ViewUserData },
            { "Volver al menú principal", BackMenu}

        };
            
        return options;
    }


    private void ExitMenu()
    {
        exit = true;
    }

    private void BackMenu()
    {
        back = true;
    }
}