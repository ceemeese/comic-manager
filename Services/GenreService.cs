namespace Services;

using Models;
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
            Console.WriteLine("___NUEVO GÉNERO___");
            Console.WriteLine("Nombre: ");
            string name = Console.ReadLine();

            if (genres.Any(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidComicException("Error: Ya existe un género con el mismo nombre");
            }

            Console.WriteLine("Description: ");
            string description = Console.ReadLine();

            int priority;
            while (true)
            {
                Console.WriteLine("Prioridad: ");
                if (int.TryParse(Console.ReadLine(), out priority) && priority > 0)
                {
                    break;
                }
                Console.WriteLine("Error: La prioridad debe ser un número positivo");
            }


            string icon;
            while (true)
            {
                Console.WriteLine("Icono: ");
                icon = Console.ReadLine();

                if (icon.StartsWith('#'))
                {
                    break; 
                }
                Console.WriteLine("Error: El icono debe empezar por '#'");
            }
            

            Genre genre = new Genre(name, description, priority, icon);
            genre.ShowGenreInformation();

            genres.Add(genre);

            Console.WriteLine("Género añadido correctamente");
            JsonUtils.SaveDataToJson(genres, Constants.GenresFileName);

        }
        catch (InvalidGenreException ex) 
        {
            var messageError = "InvalidGenreException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
            Console.WriteLine(messageError);
        }
    }



    public static void ShowAllGenres()
    {
        Console.WriteLine("\nListado de Géneros:");
        foreach (var genre in genres)
        {
            genre.ShowGenreInformation();
        }
    }
     


    public static void SearchGenre()
    {
        try 
        {
            Console.WriteLine("Introduce el nombre del género:");
            string name = Console.ReadLine();
            Genre genre = genres.Find(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (genre != null)
            {
                genre.ShowGenreInformation();
            }
            else
            {
                throw new InvalidGenreException("El género no existe");
            }
        }
        catch (InvalidGenreException ex) 
        {
            var messageError = "InvalidGenreException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
            Console.WriteLine(messageError);
        }
    }


    public static void DeleteGenre()
    {
        ShowAllGenres();
    
        try
        {
            Console.WriteLine("Selecciona el ID de la categoría a eliminar:");

            if (int.TryParse(Console.ReadLine(), out int IdSelected))
            {
                Genre genre = genres.Find(g => g.Id.Equals(IdSelected));
                if (genre != null){
                    genres.Remove(genre);
                    Console.WriteLine("Género eliminado correctamente");
                    ShowAllGenres();
                    JsonUtils.SaveDataToJson(genres, Constants.GenresFileName);
                }
                else{
                    throw new InvalidGenreException("El género no existe");
                }
            }   
        }
        catch(InvalidGenreException ex)
        {
            var messageError = "InvalidGenreException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
            Console.WriteLine(messageError);
        }
    }
}
