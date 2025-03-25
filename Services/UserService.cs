namespace Services;

using Models;
using Utils;
using Spectre.Console;

class UserService
{
    public static List<User> users = JsonUtils.LoadDataJson<User>(Constants.UsersFileName) ?? new List<User>();
    public static User? currentUser = null;

    public UserService()
    {

    }


    public static void AddUser()
    {

        try
        {
            AnsiConsole.MarkupLine("[bold underline]___NUEVO USUARIO___[/]");
            string name = AnsiConsole.Ask<string>("[yellow4]Nombre:[/]");

            string mail;
            while (true)
            {
                mail = AskValidInput("Correo:", ValidationUtils.IsValidMail, "El correo no es válido");

                if (!users.Any(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase)))
                {
                    break;
                }
                AnsiConsole.MarkupLine("[darkred]Error:[/] Ya existe un usuario con este mail");
            }
            
            string password = AskValidInput("Contraseña (Debe contener mínimo un número, una mayúscula y mínimo 8 carácteres):", ValidationUtils.IsValidPassword, "La contraseña no es válida");
            

            string telephone = AnsiConsole.Ask<string>("[yellow4]Teléfono:[/]");

            bool admin = false;
            if (currentUser != null && currentUser.IsAdmin)
            {
                admin = AnsiConsole.Prompt(
                new TextPrompt<bool>("[yellow4]Es administrador?[/]")
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
            var messageError = $"[darkred]InvalidUserException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }


    public static void ShowAllUsers()
    {
        AnsiConsole.MarkupLine("[yellow4]Listado de usuarios:[/]");
        var table = User.GenerateUserTable(users);
        AnsiConsole.Write(table);
    }



    public static void SearchUser()
    {
        try 
        {
            string mail = AnsiConsole.Ask<string>("[yellow4]Introduce el correo del usuario a buscar:[/]");
            User user = users.Find(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidUserException("[darkred]Error:[/] Usuario no existe");

            user.ShowUserInformation();

        }
        catch (InvalidUserException ex) 
        {
            var messageError = $"[darkred]InvalidUserException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }



    public static void DeleteUser()
    {
        ShowAllUsers();
    
        try
        {
            int idSelected = AnsiConsole.Ask<int>("[yellow4]Selecciona el ID del usuario a eliminar:[/]");

            User user = users.Find(u => u.Id.Equals(idSelected))
                ?? throw new InvalidUserException("[darkred]Error:[/] No hay ningún usuario con ese ID");

            users.Remove(user);
            AnsiConsole.MarkupLine("[green]Usuario eliminado correctamente[/]");
            ShowAllUsers();

            JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
         
        }
        catch(InvalidComicException ex)
        {
            var messageError = $"[darkred]InvalidUserException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }


    public static void ManageComicsInUserList(User loggedUser, bool isAddOperation) 
    {

        string operation = isAddOperation ? "AÑADIR" : "ELIMINAR";

        AnsiConsole.MarkupLine(isAddOperation ? "[yellow4]CÓMICS DISPONIBLES:[/] " : "[yellow4]CÓMICS PERSONALES: [/]");
        List<Comic> comicsList = isAddOperation ? ComicService.comics : loggedUser.PersonalComics;

        //Se pasa a String porque es el único tipo con el que opera Panel
        string comicsText = comicsList.Count > 0
            ? string.Join("\n", comicsList.Select(comic => $"{comic.Name}"))
            : "[grey]No hay cómics disponibles[/]";

        var panel = new Panel(comicsText)
            .Header("Lista")
            .Padding(2, 2, 2, 2)
            .BorderColor(Color.Yellow4)
            .Border(BoxBorder.Double);

        AnsiConsole.Write(panel);
        
        // Selección de cómics
        AnsiConsole.MarkupLine($"[bold]{operation} cómics:[/]");
        //List<Comic> selectedPersonalComics = new List<Comic>();

        var highlightStyle = new Style().Foreground(Color.LightSalmon1);
        var selectedPersonalComics = AnsiConsole.Prompt(
        new MultiSelectionPrompt<Comic>()
            .MoreChoicesText("[grey](Usa las feclas arriba y abajo para navegar por la lista)[/]")
            .InstructionsText("[grey][blue]Espacio[/] para seleccionar" + "[green] Enter:[/] confirmar selección" + "[darkred] Cancelar:[/] Enter sin seleccionar[/]")
            .AddChoices(comicsList)
            .NotRequired());


        if (isAddOperation)
        {
            var alreadyInList = selectedPersonalComics.Where(c => loggedUser.PersonalComics.Any(pc => pc.Name == c.Name)).ToList();
            var newComics = selectedPersonalComics.Where(c => !loggedUser.PersonalComics.Any(pc => pc.Name == c.Name)).ToList();

            if (alreadyInList.Any())
            {
                AnsiConsole.MarkupLine($"[darkorange]Los siguientes cómics ya están en tu lista: {string.Join(", ", alreadyInList.Select(c => $"'{c.Name}'"))}[/]");
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
            if (selectedPersonalComics.Any())
            {
                var sureDelete = AnsiConsole.Prompt(
                new TextPrompt<bool>("[yellow4]Seguro que quieres eliminar?[/]")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "si" : "no"));
                
                if (!sureDelete)
                {
                    return;
                }

                foreach (var comic in selectedPersonalComics)
                {
                    loggedUser.PersonalComics.Remove(comic!);
                }

                AnsiConsole.MarkupLine($"[green]Se han eliminado de tu lista personal: {string.Join(", ", selectedPersonalComics.Select(c => $"'{c!.Name}'"))}[/]");
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
            AnsiConsole.MarkupLine("[darkred]No tienes cómics personales registrados[/]");
        }
    }




    public static void Login()
    {
        AnsiConsole.MarkupLine("[bold underline]__LOGIN__[/]");


        string mail = AnsiConsole.Prompt(
        new TextPrompt<string>("[yellow4]Mail:[/]")
            .Validate(input => string.IsNullOrEmpty(input) ? ValidationResult.Error("[darkred]El mail no puede estar vacío[/]") : ValidationResult.Success())
        );

        
        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("[yellow4]Password:[/]")
            .Secret()
            .Validate(input => string.IsNullOrEmpty(input) ? ValidationResult.Error("[darkred]La contraseña no puede estar vacía[/]") : ValidationResult.Success())
        );

        User? user = users.FirstOrDefault(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase) && u.Password == password);

        if (user != null)
        {
            currentUser = user;
            AnsiConsole.MarkupLine($"[bold green]Hola, {user.Name}![/]");
            return;
        }
        AnsiConsole.MarkupLine("[bold darkred]Error:[/] Nombre de usuario o contraseña incorrectos");


    }


    public static void Logout()
    {
        var sureExit = AnsiConsole.Prompt(
            new TextPrompt<bool>("[yellow4]Seguro que quieres cerrar la sesión?[/]")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "si" : "no"));

        if (!sureExit)
            {
                return;
            }

        if (currentUser != null)
        {
            AnsiConsole.MarkupLine($"[bold purple]Hasta pronto, {currentUser.Name}![/]");
            currentUser = null;
        }
        else
        {
            AnsiConsole.MarkupLine("[darkred]No hay usuario conectado[/]");
        }
    }


    public static void ViewUserData()
    {
        currentUser?.ShowUserInformation();
    }


    public static void PutUserData() {

        if (currentUser != null)
        {
            currentUser.ShowUserInformation();

            var modifyOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Qué dato quieres modificar?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Usa las feclas arriba y abajo para navegar por la lista)[/]")
                    .AddChoices(new[] {
                        "Email", "Password", "Teléfono"
                    }));

            switch (modifyOption)
            {
                case "Email":
                string newMail;
                    while (true)
                    {
                        newMail = AskValidInput("Correo:", ValidationUtils.IsValidMail, "El correo no es válido");

                        if (!users.Any(u => u.Mail.Equals(newMail, StringComparison.OrdinalIgnoreCase)))
                        {
                            break;
                        }
                        AnsiConsole.MarkupLine("[darkred]Error:[/] Ya existe un usuario con este mail");
                    }
                    currentUser.Mail = newMail;
                    break;  
                case "Password":
                    string newPassword = AskValidInput("Contraseña (Debe contener mínimo un número, una mayúscula y mínimo 8 carácteres):", ValidationUtils.IsValidPassword, "La contraseña no es válida");
                    currentUser.Password = newPassword;
                    break;
                case "Teléfono":
                    string newTelephone = AnsiConsole.Ask<string>("[yellow4]Nuevo teléfono:[/]");
                    currentUser.Telephone = newTelephone;
                    break;
            }


            AnsiConsole.MarkupLine("[green]Usuario modificado correctamente[/]");
            currentUser.ShowUserInformation();
            JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
        }
    }


    public static string AskValidInput(string promptMessage, Func<string, bool> validationFunc, string errorMessage)
    {
        while (true)
        {
            string input = AnsiConsole.Ask<string>($"[yellow4]{promptMessage}[/]");
            if (validationFunc(input))
            {
                return input;
            }
            AnsiConsole.MarkupLine($"[darkred]Error:[/] {errorMessage}");
        }
    }

}


