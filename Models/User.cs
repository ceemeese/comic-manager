using Services;

namespace Models;
using Spectre.Console;

class InvalidUserException: Exception 
{
    public InvalidUserException(string message = ""):base(message) 
    {

    }
}

class User
{
    private static int nextId = 1;
    public int Id { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public DateTime? DateCreated { get; private set; }
    public string Telephone { get; set; }
    public bool IsAdmin { get; set; } = false;
    public List<Comic> PersonalComics { get; set; }
    



    //Constructor
    public User(string name, string mail, string password, string telephone, bool isAdmin, DateTime? dateCreated = null ) 
    {
        Id = nextId;
        nextId++;
        Name = name;
        Mail = mail;
        Password = password;
        DateCreated = dateCreated ?? DateTime.Now;
        Telephone = telephone;
        IsAdmin = isAdmin;
        PersonalComics = new List<Comic>();
    }


    //Mostrar información
    public void ShowUserInformation() 
    {

        var table = new Table().Border(TableBorder.Rounded).Title("[bold yellow]Información del Usuario[/]");
        table.AddColumn("[bold]ID[/]");
        table.AddColumn("[bold]Nombre[/]");
        table.AddColumn("[bold]Correo[/]");
        table.AddColumn("[bold]Teléfono[/]");
        table.AddColumn("[bold]Fecha registro[/]");
        table.AddColumn("[bold]Listado Cómics personales[/]");

        string comicsList = PersonalComics != null && PersonalComics.Count > 0 
            ? string.Join(", ", PersonalComics.Select(c => c.Name))
            : "No tiene cómics";


        if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
        {
            table.AddColumn("[bold]Es Admin?[/]");
            string admin = IsAdmin ? "[green]Sí[/]" : "[red]No[/]";
            table.AddRow(Id.ToString(), Name, Mail, Telephone, DateCreated.ToString()!, comicsList, admin);
        }
        else
        {
            table.AddRow(Id.ToString(), Name, Mail, Telephone, DateCreated.ToString()!, comicsList);
        }

        AnsiConsole.Write(table);
    }
    

    public static Table GenerateUserTable(List<User> users)
    {
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[bold]ID[/]");
        table.AddColumn("[bold]Nombre[/]");
        table.AddColumn("[bold]Correo[/]");
        table.AddColumn("[bold]Telefono[/]");
        table.AddColumn("[bold]Fecha registro[/]");
        table.AddColumn("[bold]Es admin?[/]");
        table.AddColumn("[bold]Número de Cómics Personales[/]");
        table.AddColumn("[bold]Listado de Cómics Personales[/]");

        foreach (var user in users)
        {
            string admin = user.IsAdmin ? "Sí" : "No"; 
            string personalComicsNames = user.PersonalComics != null && user.PersonalComics.Any() 
                ? string.Join(", ", user.PersonalComics.Select(c => c.Name)) 
                : "No tiene cómics";
                

            table.AddRow(
                user.Id.ToString(),
                user.Name,
                user.Mail,
                user.Telephone,
                user.DateCreated.ToString()!,
                admin,
                user.PersonalComics!.Count.ToString(),
                personalComicsNames
            );
        }

        return table;
    }
}
