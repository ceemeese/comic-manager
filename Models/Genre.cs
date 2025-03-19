namespace Models;
using Spectre.Console;

class InvalidGenreException: Exception 
{
    public InvalidGenreException(string message = ""):base(message) 
    {

    }
}

class Genre
{
    //Variable para autoincremento
    private static int nextId = 1;

    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }  
    public int Priority{ get; set; }
    public string Icon { get; set; }
    public List<string> Comics { get; set; }
    public DateTime? DateCreated { get; private set; }

    public override string ToString()
    {
        return Name;
    }
    

    public Genre (string name, string description, int priority, string icon, DateTime? dateCreated = null) {
        Id = nextId;
        nextId++;
        Name = name;
        Description = description;
        Priority = priority;
        Icon = icon;
        Comics = Comics = new List<string>();
        DateCreated = dateCreated ?? DateTime.Now;
        
    }

    public void ShowGenreInformation()
    {
        AnsiConsole.Write(new Rule($"[yellow4]Detalles del género[/]").RuleStyle("green"));

        string comicsText = string.Join("\n", Comics 
            .OrderBy(comic => comic)
            .Select(comic => $"• {comic}"));

        string genreInformationText = 
            $"[bold yellow4]{Name}[/]\n\n" +
            $"[bold]Descripción:[/] {Description}\n" +
            $"[bold]Icono:[/] {Icon}\n" +
            $"[bold]Fecha Creación:[/] {DateCreated:g}";


        // Crear layout
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Info").Ratio(2),
                new Layout("Comics").Ratio(1));

        layout["Comics"].Update(
            new Panel(
                Align.Center(
                    new Markup(comicsText),
                    VerticalAlignment.Middle))
                    .Border(BoxBorder.Double)
                    .BorderColor(Color.Yellow4)
                    .Header("Cómics")
                    .Expand());
        
        layout["Info"].Update(
            new Panel(
                Align.Center(
                    new Markup(genreInformationText),
                    VerticalAlignment.Middle))
                    .Border(BoxBorder.Double)
                    .BorderColor(Color.Yellow4)
                    .Header("[bold yellow4]Información del Género[/]")
                    .Expand());

        AnsiConsole.Write(layout);

    }
}

