namespace Services;

using Models;
using Utils;
using Spectre.Console;


class ComicService
{
    //public static List<Comic> comics = new List<Comic>();
    public static List<Comic> comics = JsonUtils.LoadDataJson<Comic>(Constants.ComicsFileName) ?? new List<Comic>();


    public ComicService()
    {
        //comics = new List<Comic>();
    }


    public static void AddComic() 
    {
        try
        {
            //Control de géneros antes de preguntar. Si no hay géneros, no se pueden añadir cómics
            if (GenreService.genres.Count == 0)
            {
                throw new InvalidComicException("[darkred]No hay géneros disponibles. Añade algunos géneros antes de seleccionar[/]");
            }


            AnsiConsole.MarkupLine("[bold underline]___NUEVO CÓMIC___[/]");
            string name = AnsiConsole.Ask<string>("[yellow4]Nombre:[/]");
            string author = AnsiConsole.Ask<string>("[yellow4]Autor:[/]");

            if (comics.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Author.Equals(author, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidComicException("[darkred]Error:[/] Ya existe un cómic con el mismo nombre y autor en la lista global");
            }

            string publisher = AnsiConsole.Ask<string>("[yellow4]Editorial:[/]");

            
            int yearPublished = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow4]Año de Publicación:[/]")
                    .Validate(y => y >= 1896 && y <= DateTime.Now.Year ? ValidationResult.Success() : ValidationResult.Error("[red]El año debe estar entre 1896 y el actual[/]")
            ));


            decimal price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[yellow4]Precio:[/]")
                .Validate(p => p > 0 ? ValidationResult.Success() : ValidationResult.Error("[darkred]El precio debe ser un número positivo[/]")
            ));

            var isForAdults = AnsiConsole.Prompt(
                new TextPrompt<bool>("[yellow4]Es para adultos?[/]")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "si" : "no"));

            var highlightStyle = new Style().Foreground(Color.LightSalmon1);
            List<Genre> selectedGenres = new List<Genre>();
            selectedGenres = AnsiConsole.Prompt(
                new MultiSelectionPrompt<Genre>()
                    .Title("[yellow4]Selecciona los géneros:[/]")
                    .MoreChoicesText("[grey](Usa las feclas arriba y abajo para navegar por la lista)[/]")
                    .InstructionsText("[grey][blue]Espacio[/] para seleccionar" +"[green] Enter:[/] confirmar selección[/]")
                    .AddChoices(GenreService.genres));


            Comic.ComicType selectedType = AnsiConsole.Prompt(new SelectionPrompt<Comic.ComicType>()
                .Title("[yellow4]Selecciona el tipo de cómic:[/]")
                .AddChoices(Enum.GetValues<Comic.ComicType>()));

            Comic comic = new Comic(name, author, publisher, yearPublished, price, isForAdults, selectedGenres, selectedType);
            comic.Genres = selectedGenres;
            comic.ShowComicInformation();

            foreach (var genre in selectedGenres)
            {
                genre.Comics.Add(comic.Name);
            }

            comics.Add(comic);
            JsonUtils.SaveDataToJson(comics, Constants.ComicsFileName);
            JsonUtils.SaveDataToJson(GenreService.genres, Constants.GenresFileName);
                
        }
        catch (InvalidComicException ex) 
        {
            var messageError = $"[darkred]InvalidComicException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }      




    public static void ShowAllComics()
    {
        
        AnsiConsole.MarkupLine("[yellow4]Listado de Cómics:[/]");
        if (comics == null || comics.Count == 0)
        {
            AnsiConsole.MarkupLine("[darkred]No hay cómics disponibles[/]");
            return;
        }

    // Generar la tabla de cómics
        var table = Comic.GenerateComicTable(comics);

    // Mostrar la tabla con todos los cómics
        AnsiConsole.Write(table);
    }


    
    public static void SearchComic()
    {

        if (comics == null || comics.Count == 0)
        {
            AnsiConsole.MarkupLine("[darkred]No hay cómics disponibles[/]");
            return;
        }

        try 
        {
            var name = AnsiConsole.Ask<string>("[yellow4]Introduce el nombre de cómic:[/]");

            Comic comic = comics.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidComicException("[darkred]Cómic no encontrado[/]");
        
            
            AnsiConsole.MarkupLine("[bold green]Cómic encontrado:[/]");
            comic.ShowComicInformation();
        

        }
        catch (InvalidComicException ex) 
        {
            var messageError = $"[darkred]InvalidComicException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }




    public static void DeleteComic()
    {
        if (comics == null || comics.Count == 0)
        {
            AnsiConsole.MarkupLine("[darkred]No hay cómics disponibles[/]");
            return;
        }

        ShowAllComics();
    
        try
        {
            List<Comic> selectedComics = new List<Comic>();

            var highlightStyle = new Style().Foreground(Color.LightSalmon1);
            selectedComics = AnsiConsole.Prompt(
            new MultiSelectionPrompt<Comic>()
            .Title("[yellow4]Selecciona los cómics a eliminar:[/]")
            .MoreChoicesText("[grey](Usa las feclas arriba y abajo para navegar por la lista)[/]")
            .InstructionsText("[grey][blue]Espacio[/] para seleccionar" + "[green] Enter:[/] confirmar selección" + "[darkred] Cancelar:[/] Enter sin seleccionar[/]")
            .AddChoices(ComicService.comics)
            .NotRequired());

            if (selectedComics.Any())
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

                foreach (var comic in selectedComics)
                {
                    comics.Remove(comic);
                }
                //comics.Remove(comic);
                AnsiConsole.MarkupLine("Cómic/s eliminados/s correctamente");
                ShowAllComics();
                JsonUtils.SaveDataToJson(comics, Constants.ComicsFileName);
                JsonUtils.SaveDataToJson(GenreService.genres, Constants.GenresFileName);
            }

        }
        catch(InvalidComicException ex)
        {
            var messageError = $"[darkred]InvalidComicException:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[darkred]ExceptionError:[/] {ex.Message}";
            AnsiConsole.MarkupLine(messageError);
        }
    }

}