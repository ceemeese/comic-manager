namespace Services;

using Models;
using Utils;
using Spectre.Console;

class UserService
{
    public static List<User> users = JsonUtils.LoadDataJson<User>(Constants.UsersFileName) ?? new List<User>();
    public static User? currentUser = null;
    //public static List<User> users = new List<User>();


    public UserService()
    {

    }



    public static void AddUser()
    {

        try
        {
            AnsiConsole.MarkupLine("[bold underline]___NUEVO USUARIO___[/]");
            string name = AnsiConsole.Ask<string>("[cyan]Nombre:[/]");

            
            string mail;
            while(true)
            {
                mail = AnsiConsole.Ask<string>("[cyan]Correo:[/]");
                if (ValidationUtils.IsValidMail(mail))
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Error: El correo no es válido[/]");
                }
            }

            if (users.Any(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase)))
            {
            throw new InvalidComicException("[red]Error: Ya existe un usuario con este mail[/]");
            }


            string password;
            while(true)
            {
                password = AnsiConsole.Ask<string>("[cyan]Contraseña (Debe contener mínimo un número, una mayúscula y mínimo 8 carácteres):[/]");
                if (ValidationUtils.IsValidPassword(password))
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Error: La contraseña no és válida[/]");
                }
            }

            string telephone = AnsiConsole.Ask<string>("[cyan]Teléfono:[/]");

            bool admin = false;
            if (currentUser != null && currentUser.IsAdmin)
            {
                admin = AnsiConsole.Prompt(
                new TextPrompt<bool>("[cyan]Es administrador?[/]")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "si" : "no"));
            }


            User user = new User(name, mail, password, telephone, admin);
            AnsiConsole.MarkupLine("[green]Usuario registrado correctamente[/]");
            user.ShowUserInformation();
            users.Add(user);
            JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
        } 
        catch (InvalidUserException ex) 
        {
            var messageError = $"[red]InvalidUserException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
    }


    public static void ShowAllUsers()
    {
        AnsiConsole.MarkupLine("[cyan]Listado de usuarios:[/]");
        var table = User.GenerateUserTable(users);
        AnsiConsole.Write(table);
    }



    public static void SearchUser()
    {
        try 
        {
            string mail = AnsiConsole.Ask<string>("[cyan]Introduce el correo del usuario a buscar:[/]");
            User user = users.Find(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidUserException("[red]Error: Usuario no existe[/]");

            user.ShowUserInformation();

        }
        catch (InvalidUserException ex) 
        {
            var messageError = $"[red]InvalidUserException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
    }



    public static void DeleteUser()
    {
        ShowAllUsers();
    
        try
        {
            int idSelected = AnsiConsole.Ask<int>("[cyan]Selecciona el ID del usuario a eliminar:[/]");

            User user = users.Find(u => u.Id.Equals(idSelected))
                ?? throw new InvalidUserException("[red]Error: No hay ningún usuario con ese ID[/]");

            users.Remove(user);
            AnsiConsole.MarkupLine("[green]Usuario eliminado correctamente.[/]");
            ShowAllUsers();

            JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
         
        }
        catch(InvalidComicException ex)
        {
            var messageError = $"[red]InvalidUserException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
    }


    public static void ManageComicsInUserList(User loggedUser, bool isAddOperation) 
    {

        string operation = isAddOperation ? "Añadir" : "Eliminar";

        AnsiConsole.MarkupLine(isAddOperation ? "[cyan]Cómics disponibles:[/] " : "[cyan]Tus cómics personales: [/]");
        List<Comic> comicsList = isAddOperation ? ComicService.comics : loggedUser.PersonalComics;
        
            
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[bold]ID[/]").AddColumn("[bold]Nombre[/]");

        foreach (var comic in comicsList)
        {
            table.AddRow(comic.Id.ToString(), comic.Name);
        }
        AnsiConsole.Write(table);

        // Selección de cómics
        AnsiConsole.MarkupLine($"[bold]{operation} cómics:[/]");
        //List<Comic> selectedPersonalComics = new List<Comic>();

        var selectedPersonalComics = AnsiConsole.Prompt(
        new MultiSelectionPrompt<Comic>()
            .Title("[cyan]Selecciona los cómics:[/]")
            .InstructionsText("[grey](Usa las flechas y espacio para seleccionar, enter para confirmar)[/]")
            .AddChoices(comicsList));


        if (isAddOperation)
        {
            var alreadyInList = selectedPersonalComics.Where(c => loggedUser.PersonalComics.Any(pc => pc.Name == c.Name)).ToList();
            var newComics = selectedPersonalComics.Where(c => !loggedUser.PersonalComics.Any(pc => pc.Name == c.Name)).ToList();

            if (alreadyInList.Any())
            {
                AnsiConsole.MarkupLine($"[yellow]Los siguientes cómics ya están en tu lista: {string.Join(", ", alreadyInList.Select(c => $"'{c.Name}'"))}[/]");
            }

            if (newComics.Any())
            {
                loggedUser.PersonalComics.AddRange(newComics);
                AnsiConsole.MarkupLine($"[green]Se han añadido a tu lista personal: {string.Join(", ", newComics.Select(c => $"'{c.Name}'"))}[/]");
                JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
            }
        }
        else
        {
            var comicsToRemove = selectedPersonalComics
                .Where(c => loggedUser.PersonalComics.Any(pc => pc.Name == c.Name))
                .ToList();

            if (comicsToRemove.Any())
            {
                foreach (var comic in comicsToRemove)
                {
                    loggedUser.PersonalComics.Remove(comic!);
                }

                AnsiConsole.MarkupLine($"[green]Se han eliminado de tu lista personal: {string.Join(", ", comicsToRemove.Select(c => $"'{c!.Name}'"))}[/]");
                JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
            }
        }
    }




    // Mostrar los cómics del usuario
    public static void ShowUserComics(User user)
    {

        if (user.PersonalComics.Count > 0)
        {
            AnsiConsole.MarkupLine($"Hola [bold][green]{user.Name}[/][/], esta es tu lista de cómics:");

            var table = new Table()
                .AddColumn("[bold]Nombre[/]")
                .AddColumn("[bold]Autor[/]"); 

            foreach (var comic in user.PersonalComics)
            {
                table.AddRow(comic.Name, comic.Author);
            }

            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No tienes cómics personales registrados[/]");
        }
    }




    public static void Login()
    {
        AnsiConsole.MarkupLine("[bold underline]__LOGIN__[/]");

        string mail = AnsiConsole.Prompt(
        new TextPrompt<string>("[cyan]Mail:[/]")
            .Validate(input => string.IsNullOrEmpty(input) ? ValidationResult.Error("[red]El mail no puede estar vacío.[/]") : ValidationResult.Success())
        );


        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("[cyan]Password:[/]")
            .Secret()
            .Validate(input => string.IsNullOrEmpty(input) ? ValidationResult.Error("[red]La contraseña no puede estar vacía.[/]") : ValidationResult.Success())
        );

        User? user = users.FirstOrDefault(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase) && u.Password == password);

        if (user != null)
        {
            currentUser = user;
            AnsiConsole.MarkupLine($"[bold green]Hola, {user.Name}![/]");
            return;
        }
        AnsiConsole.MarkupLine("[bold red]Error: Nombre de usuario o contraseña incorrectos.[/]");


    }


    public static void Logout()
    {
        if (currentUser != null)
        {
            AnsiConsole.MarkupLine($"[bold purple]Hasta pronto, {currentUser.Name}![/]");
            currentUser = null;
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No hay usuario conectado[/]");
        }
    }


    public static void ViewUserData()
    {
        currentUser?.ShowUserInformation();
    }

}