namespace Models;
using Spectre.Console;

class InvalidComicException: Exception 
{
    public InvalidComicException(string message = ""):base(message) 
    {

    }
}

class Comic 
{
    //Variable para autoincremento
    private static int nextId = 1;

    public int Id { get; private set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public int YearPublished { get; set; }
    public decimal Price { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsForAdults { get; set; } = true;
    public List<Genre> Genres { get; set; } = new List<Genre>();
    public ComicType Type { get; set; }
    


    public enum ComicType
    {
        Americano = 1,
        Europeo = 2,
        Manga = 3,
        Manhwa = 4,
        Manhua = 5,
        Latinoamericano = 6,
        Webcomic = 7
    }

    //Constructor
    public Comic (string name, string author, string publisher, int yearPublished, decimal price, bool isForAdults, List<Genre> genres, ComicType type)
    {
        Id = nextId;
        nextId++;
        Name = name;
        Author = author;
        Publisher = publisher;
        YearPublished = yearPublished;
        Price = price;
        IsForAdults = isForAdults;
        Genres = genres ?? new List<Genre>();
        Type = type;
    }


    public void ShowComicInformation()
    {
        string adults = IsForAdults ? "Sí" : "No"; 

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[bold]ID[/]");
        table.AddColumn("[bold]Nombre[/]");
        table.AddColumn("[bold]Autor[/]");
        table.AddColumn("[bold]Año[/]");
        table.AddColumn("[bold]Precio[/]");
        table.AddColumn("[bold]Es para adultos?[/]");
        table.AddColumn("[bold]Tipo de Cómic[/]");
        table.AddColumn("[bold]Géneros[/]");

        string genreList = Genres != null && Genres.Any() ? string.Join(", ", Genres.Select(g => g.Name)) : "No tiene géneros";

        table.AddRow(Id.ToString(), Name, Author, YearPublished.ToString(), Price.ToString("C"), adults, Type.ToString(), genreList);

        AnsiConsole.Write(table);

    }


    public static Table GenerateComicTable(List<Comic> comics)
    {
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[bold]ID[/]");
        table.AddColumn("[bold]Nombre[/]");
        table.AddColumn("[bold]Autor[/]");
        table.AddColumn("[bold]Año[/]");
        table.AddColumn("[bold]Precio[/]");
        table.AddColumn("[bold]Es para adultos?[/]");
        table.AddColumn("[bold]Tipo de Cómic[/]");
        table.AddColumn("[bold]Géneros[/]");

        foreach (var comic in comics)
        {
            string adults = comic.IsForAdults ? "Sí" : "No"; 
            string genreNames = comic.Genres != null && comic.Genres.Any() 
                ? string.Join(", ", comic.Genres.Select(g => g.Name)) 
                : "No tiene géneros";

            table.AddRow(
                comic.Id.ToString(),
                comic.Name,
                comic.Author,
                comic.YearPublished.ToString(),
                comic.Price.ToString("C"),
                adults,
                comic.Type.ToString(),
                genreNames
            );
        }

        return table;
    }

}