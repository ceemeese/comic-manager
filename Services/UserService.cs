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
                    AnsiConsole.WriteLine("[red]Error: El correo no es válido[/]");
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
                    AnsiConsole.WriteLine("[red]Error: La contraseña no és válida[/]");
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
            AnsiConsole.WriteLine("[green]Usuario registrado correctamente[/]");
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
            AnsiConsole.WriteLine(messageError);
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
            AnsiConsole.WriteLine(messageError);
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
            AnsiConsole.WriteLine(messageError);
        }
    }


    public static void ManageComicsInUserList(User loggedUser, bool isAddOperation) 
    {

        List<Comic> selectedComics = new List<Comic>();
        string operation = isAddOperation ? "añadir" : "eliminar";

        while(true)
        {
            AnsiConsole.MarkupLine(isAddOperation ? "[cyan]Cómics disponibles:[/] " : "[cyan]Tus cómics personales: [/]");
            List<Comic> comicsList = isAddOperation ? ComicService.comics : loggedUser.PersonalComics;
            
            
            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("[bold]ID[/]").AddColumn("[bold]Nombre[/]");

            foreach (var comic in comicsList)
            {
                table.AddRow(comic.Id.ToString(), comic.Name);
            }

            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine($"[bold]{operation} cómics:[/]");
            
            string answer = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Introduce los números de los cómics que quieres seleccionar, separados por comas (ejemplo: 1,3,5):[/]") 
                    .AllowEmpty()  // Permite que el usuario deje el campo vacío
            );


            if (string.IsNullOrWhiteSpace(answer))
            {
                AnsiConsole.MarkupLine("[red]Error: No has seleccionado ningún cómic. Intenta de nuevo.[/]");
                continue;
            }

            string[] comicIndexArray = answer.Split(',');
            selectedComics.Clear();

            bool isValid = true;

            foreach (string index in comicIndexArray)
            {
                if (int.TryParse(index.Trim(), out int comicId) && comicId > 0 && ComicService.comics.Any(c => c.Id == comicId))
                {
                    Comic comic = ComicService.comics.FirstOrDefault(c => c.Id == comicId)!;

                    if (isAddOperation && loggedUser.PersonalComics.Any(c => c.Id == comic.Id))
                    {
                        AnsiConsole.MarkupLine($"[yellow]El cómic '{comic.Name}' ya está en tu lista personal.[/]");
                    }
                    else
                    {
                        selectedComics.Add(comic);
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Error: La opción '{index}' marcada no es correcta. Intenta de nuevo.[/]");
                    isValid = false;
                    break;
                }
            }

            if (isValid && selectedComics.Count > 0)
            {
                break;
            }
        }

        foreach (var comic in selectedComics)
        {
            if (isAddOperation)
            {
                loggedUser.PersonalComics.Add(comic);
                AnsiConsole.MarkupLine($"[green]El cómic '{comic.Name}' se ha añadido a tu lista personal.[/]");
                JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
            }
            else
            {
                loggedUser.PersonalComics.Remove(comic);
                AnsiConsole.MarkupLine($"[green]El cómic '{comic.Name}' ha sido eliminado de tu lista personal.[/]");
                JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
            }
        }

    }



    // Mostrar los cómics del usuario
    public static void ShowUserComics(User user)
    {
        AnsiConsole.MarkupLine($"Hola [bold]{user.Name}[/], esta es tu lista de cómics:");

        if (user.PersonalComics.Count > 0)
        {
            var table = new Table()
                .AddColumn("[bold]Nombre[/]")
                .AddColumn("[bold]Autor[/]"); 

            foreach (var comic in user.PersonalComics)
            {
                table.AddRow(comic.Name);
                table.AddRow(comic.Author);
            }

            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No tienes cómics registrados[/]");
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