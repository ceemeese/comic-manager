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
                throw new InvalidComicException("[red]No hay géneros disponibles. Añade algunos géneros antes de seleccionar[/]");
            }


            AnsiConsole.MarkupLine("[bold underline]___NUEVO CÓMIC___[/]");
            string name = AnsiConsole.Ask<string>("[cyan]Nombre:[/]");
            string author = AnsiConsole.Ask<string>("[cyan]Autor:[/]");

            if (comics.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Author.Equals(author, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidComicException("[red]Error: Ya existe un cómic con el mismo nombre y autor en la lista global[/]");
            }

            string publisher = AnsiConsole.Ask<string>("[cyan]Editorial:[/]");

            
            int yearPublished = AnsiConsole.Prompt(
                new TextPrompt<int>("[cyan]Año de Publicación:[/]")
                    .Validate(y => y >= 1896 && y <= DateTime.Now.Year ? ValidationResult.Success() : ValidationResult.Error("[red]El año debe estar entre 1896 y el actual.[/]")
            ));


            decimal price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[cyan]Precio:[/]")
                .Validate(p => p > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]El precio debe ser un número positivo.[/]")
            ));

            var isForAdults = AnsiConsole.Prompt(
                new TextPrompt<bool>("[cyan]Es para adultos?[/]")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "si" : "no"));

            List<Genre> selectedGenres = new List<Genre>();
            while(true)
            {
                var genreSelection = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<Genre>()
                        .Title("[cyan]Selecciona los géneros:[/]")
                        .InstructionsText("[grey](Usa las flechas y espacio para seleccionar, enter para confirmar.)[/]")
                        .AddChoices(GenreService.genres));


                if (genreSelection.Count > 0)
                {
                    selectedGenres = genreSelection;
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Debes seleccionar al menos un género.[/]");
                }
            }


            Comic.ComicType selectedType = AnsiConsole.Prompt(new SelectionPrompt<Comic.ComicType>()
                .Title("[cyan]Selecciona el tipo de cómic:[/]")
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
            var messageError = $"[red]InvalidComicException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.WriteLine(messageError);
        }
    }      




    public static void ShowAllComics()
    {
        
        AnsiConsole.MarkupLine("[cyan]Listado de Cómics:[/]");
        if (comics == null || comics.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay cómics disponibles.[/]");
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
            AnsiConsole.MarkupLine("[red]No hay cómics disponibles.[/]");
            return;
        }

        try 
        {
            var name = AnsiConsole.Ask<string>("[cyan]Introduce el nombre de cómic:[/]");

            Comic comic = comics.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidComicException("[red]Cómic no encontrado.[/]");
        
            
            AnsiConsole.MarkupLine("[bold green]Cómic encontrado:[/]");
            comic.ShowComicInformation();
        

        }
        catch (InvalidComicException ex) 
        {
            var messageError = $"[red]InvalidComicException: {ex.Message}[/]";
            AnsiConsole.MarkupLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.WriteLine(messageError);
        }
    }




    public static void DeleteComic()
    {
        if (comics == null || comics.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay cómics disponibles.[/]");
            return;
        }

        ShowAllComics();
    
        try
        {
            AnsiConsole.WriteLine("Selecciona el ID del comic a eliminar:");

            if (int.TryParse(Console.ReadLine(), out int IdSelected))
            {
                Comic comic = comics.Find(c => c.Id.Equals(IdSelected))
                    ?? throw new InvalidComicException("[red]No hay ningún cómic con el ID introducido[/]");
                
                comics.Remove(comic);
                AnsiConsole.WriteLine("Cómic eliminado correctamente");
                ShowAllComics();
                JsonUtils.SaveDataToJson(comics, Constants.ComicsFileName);
                JsonUtils.SaveDataToJson(GenreService.genres, Constants.GenresFileName);

            }   
        }
        catch(InvalidComicException ex)
        {
            var messageError = $"[red]InvalidComicException: {ex.Message}[/]";
            AnsiConsole.WriteLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = $"[red]ExceptionError: {ex.Message}[/]";
            AnsiConsole.WriteLine(messageError);
        }
    }

}