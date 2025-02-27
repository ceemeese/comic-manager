namespace Services;

using Models;
using Spectre.Console;
using Utils;

class GenreService
{
    public static List<Genre> genres = JsonUtils.LoadDataJson<Genre>(Constants.GenresFileName) ?? new List<Genre>();
    //public static List<Genre> genres = new List<Genre>();
    
    public GenreService()
    {
        //genres = new List<Genre>();
    }


    public static void AddGenre()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold underline]___NUEVO GÉNERO___[/]");
            string name = AnsiConsole.Ask<string>("[cyan]Nombre:[/]");

            if (genres.Any(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidComicException("[red]Error: Ya existe un género con el mismo nombre[/]");
            }

            string description = AnsiConsole.Ask<string>("[cyan]Descripción:[/]");

            int priority = AnsiConsole.Prompt(
                new TextPrompt<int>("[cyan]Prioridad:[/]")
                    .Validate(num => num > 0 ? ValidationResult.Success() : ValidationResult.Error("La prioridad debe ser un número positivo"))
            );

            string icon = AnsiConsole.Prompt(
                new TextPrompt<string>("[cyan]Icono (debe empezar con '#'):[/]")
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
            var messageError = $"[red]InvalidGenreException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.WriteLine(messageError);
        }
    }



    public static void ShowAllGenres()
    {
        if (genres == null || genres.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay géneros disponibles.[/]");
            return;
        }

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[bold]Nombre[/]");

        foreach (var genre in genres.OrderBy(g => g.Priority))
        {
            table.AddRow(genre.Name);
        }

        AnsiConsole.Write(table);
    }
     


    public static void SearchGenre()
    {

        if (genres == null || genres.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay géneros disponibles.[/]");
            return;
        }


        try 
        {
            string name = AnsiConsole.Ask<string>("[cyan]Introduce el nombre del género:[/]");
            Genre genre = genres.Find(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidGenreException("[red]El género no existe[/]");

            genre.ShowGenreInformation();

        }
        catch (InvalidGenreException ex) 
        {
            var messageError = $"[red]InvalidGenreException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.WriteLine(messageError);
        }
    }


    public static void DeleteGenre()
    {

        if (genres == null || genres.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No hay géneros disponibles para eliminar.[/]");
                return;
            }

        ShowAllGenres();
    
        try
        {
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("[cyan]Selecciona el ID del género a eliminar:[/]")
                    .Validate(num => genres.Any(g => g.Id == num) ? ValidationResult.Success() : ValidationResult.Error("El género no existe"))
            );

            Genre genre = genres.Find(g => g.Id.Equals(id))
                ?? throw new InvalidGenreException("[red]El género no existe[/]");


            genres.Remove(genre);
            AnsiConsole.WriteLine("[green]Género eliminado correctamente[/]");
            ShowAllGenres();
            JsonUtils.SaveDataToJson(genres, Constants.GenresFileName);
    
        }
        catch(InvalidGenreException ex)
        {
            var messageError = $"[red]InvalidGenreException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.WriteLine(messageError);
        }
    }
}
