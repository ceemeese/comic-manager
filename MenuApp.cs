using Services;
using Spectre.Console;


class MenuApp
{
    private readonly GenreService _genreService;
    private readonly UserService _userService;
    private readonly ComicService _comicService;

    private bool exit = false;
    private bool back = false;


    public MenuApp(GenreService genreService, UserService userService, ComicService comicService)
    {
        _genreService = genreService;
        _userService = userService;
        _comicService = comicService;
        
        AnsiConsole.Write(
            new FigletText("Comic Manager")
            .LeftJustified()
            .Color(Color.LightSalmon1));
    }
    
    public void ShowMenu()
    {
        //int option = 0;
        var option = "";
        
        
        do
        {
            var options = GetOptionsMenu();
            var highlightStyle = new Style().Foreground(Color.LightSalmon1);

            WriteMenuRule("MENU PRINCIPAL");

            option = ShowSelectionMenu(options, "Selecciona una opción");
            options[option].Invoke();

        }

        while(!exit);

        AnsiConsole.MarkupLine("[bold green]¡Hasta pronto![/]");
    }



    private void ShowGenreMenu()
    {
        back = false;
        string option = "";

        do
        {

            var options = GetOptionsGenreMenu();
            Console.Clear();
            
            WriteMenuRule("MENU GÉNEROS");

            option = ShowSelectionMenu(options, "Selecciona una opción");
            options[option].Invoke();

            if (option != "Volver al menú principal")
            {
                AnsiConsole.Write(new Markup("[bold]Presiona [green]Enter[/] para continuar...[/]"));
                Console.ReadLine();
            }

            Console.Clear();

        }
        while (!back);

    }



    private void ShowComicMenu()
    {
        back = false;
        string option = "";

        do
        {

            var options = GetOptionsComicMenu();
            Console.Clear();

            WriteMenuRule("MENU CÓMICS");
            option = ShowSelectionMenu(options, "Selecciona una opción");

            options[option].Invoke();
            
            if (option != "Volver al menú principal")
            {
                AnsiConsole.Write(new Markup("[bold]Presiona [green]Enter[/] para continuar...[/]"));
                Console.ReadLine();
            }
            Console.Clear();

        }
        while (!back);

    }



    private void ShowUserMenu()
    {
        back = false;
        string option = "";

        do
        {
            var options = GetOptionsUserMenu();
            AnsiConsole.Clear();

            WriteMenuRule("MENU USUARIOS");
            option = ShowSelectionMenu(options, "Selecciona una opción");

            options[option].Invoke();
            if (option != "Volver al menú principal")
            {
                AnsiConsole.Write(new Markup("[bold]Presiona [green]Enter[/] para continuar...[/]"));
                Console.ReadLine();
            }
            Console.Clear();
        
        }
        while (!back);

    }



    private void ShowPrivateMenu()
    {
        back = false;
        string option = "";

        do
        {
            var options = GetOptionsPrivateMenu();
            AnsiConsole.Clear();

            WriteMenuRule("ZONA PRIVADA");
            option = ShowSelectionMenu(options, "Selecciona una opción");

            options[option].Invoke();
            if (option != "Volver al menú principal")
            {
                AnsiConsole.Write(new Markup("[bold]Presiona [green]Enter[/] para continuar...[/]"));
                Console.ReadLine();
            }
            Console.Clear();
            
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
            options["Registrarse"] = _userService.AddUser;
        }
        else
        {
            if (UserService.currentUser.IsAdmin)
            {
                options["Usuarios"] = ShowUserMenu;
            }
            options["Zona Privada"] = ShowPrivateMenu;
            options["Cerrar Sesión"] = UserService.Logout;
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
            { "Buscar género", _genreService.SearchGenre },
        };

        if (UserService.currentUser?.IsAdmin == true)
        {
            options["Añadir género"] = _genreService.AddGenre;
            options["Eliminar género"] = _genreService.DeleteGenre;
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
            { "Buscar cómic", _comicService.SearchComic},
        };

        if (UserService.currentUser != null)
        {
            options["Añadir cómic"] = _comicService.AddComic;

            if (UserService.currentUser.IsAdmin) 
            {
                options["Eliminar cómic"] = _comicService.DeleteComic;
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
            { "Añadir usuario", _userService.AddUser },
            { "Listar usuarios", UserService.ShowAllUsers },
            { "Buscar usuario", _userService.SearchUser },
            { "Eliminar usuario", _userService.DeleteUser},
            { "Volver al menú principal", BackMenu}
        };
            
        return options;
    }


    private Dictionary<string, Action> GetOptionsPrivateMenu()
    {
        var options = new Dictionary<string, Action>

        {
            { "Añadir cómic a la lista personal", () => _userService.ManageComicsInUserList(UserService.currentUser!, true) },
            { "Eliminar cómic de la lista personal", () => _userService.ManageComicsInUserList(UserService.currentUser!, false) },
            { "Ver mi lista personal de cómics",() => UserService.ShowUserComics(UserService.currentUser!)},
            { "Ver mis datos personales", UserService.ViewUserData },
            { "Modificar datos personales", _userService.PutUserData },
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

    private string ShowSelectionMenu(Dictionary<string, Action> options, string title)
    {
        var highlightStyle = new Style().Foreground(Color.LightSalmon1);
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[bold yellow4]{title}[/]")
                .AddChoices(options.Keys)
                .HighlightStyle(highlightStyle));
    }

    private void WriteMenuRule(string menuTitle)
    {
        AnsiConsole.Write(new Rule($"[bold yellow4] {menuTitle} [/]").RuleStyle("lightsalmon1"));
    }
}