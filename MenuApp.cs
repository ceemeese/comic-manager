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
            .Color(Color.DarkBlue));
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
                .Title("[bold yellow4]--- MENÚ PRINCIPAL ---[/]")
                .AddChoices(options.Keys));

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

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold yellow4]--- MENÚ GÉNEROS ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();

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

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold yellow4]--- MENÚ CÓMICS ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();
        
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

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold yellow4]--- MENÚ USUARIOS ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();
        
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

            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold yellow4]--- ZONA PRIVADO ---[/]")
                .AddChoices(options.Keys));

            options[option].Invoke();
            
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