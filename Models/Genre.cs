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
    public decimal PercentageOfComics { get; private set; }
    public bool IsPopular { get; private set; } = false;

    public override string ToString()
    {
        return Name;
    }


    public Genre (string name, string description, int priority, string icon, DateTime? dateCreated = null, decimal percentageOfComics = 0, bool isPopular = false) {
        Id = nextId;
        nextId++;
        Name = name;
        Description = description;
        Priority = priority;
        Icon = icon;
        Comics =  new List<string>();
        DateCreated = dateCreated ?? DateTime.Now;
        PercentageOfComics = percentageOfComics;
        IsPopular = isPopular;
    }

    public void ShowGenreInformation()
    {
        string popularityStatus = IsPopular ? "[bold green]Popular[/]" : "[bold red]No Popular[/]";

        AnsiConsole.Write(new Rule($"[yellow4]Detalles del género[/]").RuleStyle("lightsalmon1"));

        string comicsText = string.Join("\n", Comics 
            .OrderBy(comic => comic)
            .Select(comic => $"• {comic}"));

        string genreInformationText = 
            $"[bold yellow4]{Name}[/]\n\n" +
            $"[bold]Descripción:[/] {Description}\n" +
            $"[bold]Icono:[/] {Icon}\n" +
            $"[bold]Fecha Creación:[/] {DateCreated:g}\n" +
            $"[bold]Es popular? [/] {popularityStatus}\n"+            
            $"[bold]Porcentaje de cómics de este género:[/] {PercentageOfComics}%";


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


    /*private void CalculatePercentajeComics(List<Comic> comicsTotal)
    {
        int comicTotalCount = comicsTotal.Count;
        PercentageOfComics =  (Comics.Count / comicTotalCount) * 100;
    }*/

    public void UpdatePercentage(int totalComics)
    {
        if (totalComics > 0)
        {
            PercentageOfComics = Math.Round((decimal)Comics.Count * 100 / totalComics, 2);
        }
    }


    public void UpdatePopularity()
    {
        
        decimal popularThresholdPercentage = 20;

        if (PercentageOfComics > popularThresholdPercentage )
        {
            IsPopular = true;
        }
    }
}

