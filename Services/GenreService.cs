namespace Services;

using Models;
using Spectre.Console;
using Utils;

class GenreService
{
    public static List<Genre> genres = JsonUtils.LoadDataJson<Genre>(Constants.GenresFileName) ?? new List<Genre>();
    
    
    public GenreService()
    {
        
    }


    public static void AddGenre()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold underline]___NUEVO GÉNERO___[/]");
            string name = AnsiConsole.Ask<string>("[yellow4]Nombre:[/]");

            if (genres.Any(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidComicException("[darkred]Error:[/] Ya existe un género con el mismo nombre");
            }

            string description = AnsiConsole.Ask<string>("[yellow4]Descripción:[/]");

            int priority = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow4]Prioridad:[/]")
                    .Validate(num => num > 0 ? ValidationResult.Success() : ValidationResult.Error("La prioridad debe ser un número positivo"))
            );

            string icon = AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow4]Icono (debe empezar con '#'):[/]")
                    .Validate(input => input.StartsWith('#') ? ValidationResult.Success() : ValidationResult.Error("El icono debe empezar por '#'"))
            );
            

            Genre genre = new Genre(name, description, priority, icon);
            genre.ShowGenreInformation();

            genres.Add(genre);

            AnsiConsole.MarkupLine("[green]Género añadido correctamente[/]");
            JsonUtils.SaveDataToJson(genres, Constants.GenresFileName);

        }
        catch (InvalidGenreException ex) 
        {
            var messageError = $"[darkred]InvalidGenreException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }



    public static void ShowAllGenres()
    {
        if (genres == null || genres.Count == 0)
        {
            AnsiConsole.MarkupLine("[darkred]No hay géneros disponibles[/]");
            return;
        }

        string genresText = string.Join("\n", genres
            .OrderBy(genre => genre.Name)
            .Select(genre => $"{genre.Name}"));

        var panel = new Panel(genresText)
            .Header("Géneros")
            .Padding(2, 2, 2, 2)
            .BorderColor(Color.Yellow4)
            .Border(BoxBorder.Double);

        AnsiConsole.Write(panel);

        var chart = new BarChart()
            .Width(60)
            .Label("[yellow4 bold underline]Porcentaje de cómics por género[/]\n");

        foreach (var genre in genres)
        {
            Color barColor = genre.PercentageOfComics > 50 ? Color.PaleGreen1_1 : Color.Maroon;
            chart.AddItem($"{genre.Name}\n", (double)genre.PercentageOfComics, barColor);
        }

        AnsiConsole.Write(chart);

    }
     


    public static void SearchGenre()
    {

        if (genres == null || genres.Count == 0)
        {
            AnsiConsole.MarkupLine("[darkred]No hay géneros disponibles[/]");
            return;
        }


        try 
        {
            string name = AnsiConsole.Ask<string>("[yellow4]Introduce el nombre del género:[/]");
            Genre genre = genres.Find(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidGenreException("[darkred]El género no existe[/]");

            genre.ShowGenreInformation();

        }
        catch (InvalidGenreException ex) 
        {
            var messageError = $"[darkred]InvalidGenreException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }


    public static void DeleteGenre()
    {

        if (genres == null || genres.Count == 0)
            {
                AnsiConsole.MarkupLine("[darkred]No hay géneros disponibles para eliminar[/]");
                return;
            }

        ShowAllGenres();
    
        try
        {
            List<Genre> selectedGenres = new List<Genre>();

            var highlightStyle = new Style().Foreground(Color.LightSalmon1);
            selectedGenres = AnsiConsole.Prompt(
                new MultiSelectionPrompt<Genre>()
                    .Title("[yellow4]Selecciona los géneros a eliminar:[/][grey](Usa las feclas arriba y abajo para navegar por la lista)[/]")
                    .InstructionsText("[grey][blue]Espacio[/] para seleccionar" + "[green] Enter:[/] confirmar selección" + "[darkred] Cancelar:[/] Enter sin seleccionar[/]")
                    .AddChoices(GenreService.genres)
                    .NotRequired());

            if (selectedGenres.Any())
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

                foreach (var genre in selectedGenres)
                {
                    genres.Remove(genre); 
                }
                AnsiConsole.MarkupLine("[green]Género/s eliminado/s correctamente[/]");
                ShowAllGenres();
                JsonUtils.SaveDataToJson(genres, Constants.GenresFileName);
            }
            
        }
        catch(InvalidGenreException ex)
        {
            var messageError = $"[darkred]InvalidGenreException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }


    public static void UpdateGenrePercentages(int totalComics)
    {
        foreach (var genre in genres)
        {
            genre.UpdatePercentage(totalComics);
        }
    }
}
