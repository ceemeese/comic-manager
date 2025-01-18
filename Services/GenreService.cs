namespace Services;
using Models;

class GenreService
{
    public static List<Genre> genres = new List<Genre>();

    
    public GenreService()
    {
        genres = new List<Genre>();
    }




    public static void AddGenre()
    {
        try
        {
            Console.WriteLine("___NUEVO GÉNERO___");
            Console.WriteLine("Nombre: ");
            string name = Console.ReadLine();
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
                Console.WriteLine("Error: La prioridad debe ser un número positivo.");
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
                Console.WriteLine("Error: El icono debe empezar por '#'.");
            }
            

            Genre genre = new Genre(name, description, priority, icon);
            genre.ShowGenreInformation();

            genres.Add(genre);

            Console.WriteLine("Género añadido correctamente.");

        }
        catch (InvalidGenreException ex) 
        {
            var messageError = "InvalidGenreException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
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
        Console.WriteLine("Introduce el nombre de la categoría:");
        string name = Console.ReadLine();
        Genre genre = genres.Find(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (genre != null)
        {
            genre.ShowGenreInformation();
        }
        else
        {
            Console.WriteLine("Género no encontrado.");
        }
    }
}