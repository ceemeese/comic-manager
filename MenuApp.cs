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
            
            WriteMenuRule("MENU GËNEROS");

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
            options["Registrarse"] = UserService.AddUser;
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