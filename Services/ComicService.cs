namespace Services;

using Models;
using Utils;

class ComicService
{
    public static List<Comic> comics = new List<Comic>();


    public ComicService()
    {
        comics = new List<Comic>();
    }


    public static void AddComic() 
    {
        try
        {
            //Control de géneros antes de preguntar. Si no hay géneros, no se pueden añadir cómics
            if (GenreService.genres.Count == 0)
            {
                Console.WriteLine("No hay géneros disponibles. Añade algunos géneros antes de seleccionar.");
                return;
            }


            Console.WriteLine("___NUEVO CÓMIC___");
            Console.WriteLine("Nombre: ");
            string name = Console.ReadLine();


            Console.WriteLine("Autor: ");
            string author
            = Console.ReadLine();


            Console.WriteLine("Editorial: ");
            string publisher = Console.ReadLine();


            //Validación año
            int yearPublished;
            int currentYear = DateTime.Now.Year;
            while (true)
            {
                Console.WriteLine("Año de Publicación: ");
                if (int.TryParse(Console.ReadLine(), out yearPublished) && yearPublished >= 1896 && yearPublished <= currentYear)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Error: El año debe ser entre 1896 y {currentYear}.");
                }
            }


            //Validación precio
            decimal price;
            while (true)
            {
                Console.WriteLine("Precio: ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: El precio debe ser un número positivo.");
                }
            }


            Console.WriteLine("Leído? (si/no): ");
            string answer = Console.ReadLine();
            bool isRead = answer.ToLower() == "yes" ? true : false;


            Console.WriteLine("Es para adultos? (si/no): ");
            answer = Console.ReadLine();
            bool isForAdults = answer.ToLower() == "yes" ? true : false;


            List<Genre> selectedGenres = new List<Genre>();
            while(true)
            {
                Console.WriteLine("Géneros disponibles: ");
                for (int i = 0; i < GenreService.genres.Count; i++)
                {
                    Console.WriteLine($"{GenreService.genres[i].Id}. {GenreService.genres[i].Name}");
                }

                Console.WriteLine("Introduce los números de los géneros separados por comas (ejemplo: 1,3,5):");
                answer = Console.ReadLine();


                if (string.IsNullOrWhiteSpace(answer))
                {
                    Console.WriteLine("Error: No has seleccionado ningún género. Intentelo de nuevo.");
                    continue;
                }

                string[] genreIndexArray = answer.Split(',');
                selectedGenres.Clear();

                bool isValid = true;

                foreach (string index in genreIndexArray)
                {
                    if (int.TryParse(index.Trim(), out int genreId) && genreId > 0 && GenreService.genres.Any(g => g.Id == genreId))
                    {
                        Genre genre = GenreService.genres.FirstOrDefault(g => g.Id == genreId);
                        selectedGenres.Add(genre);
                    }
                    else
                    {
                        Console.WriteLine($"Error: La opción '{index}' marcada no es correcta. Intentelo de nuevo.");
                        isValid = false;
                        break;
                    }
                }

                if (isValid && selectedGenres.Count > 0)
                {
                    break;
                }
            }


            Console.WriteLine("Tipos de cómics:");
            foreach (var comicType in Enum.GetValues(typeof(Comic.ComicType)))
            {
                Console.WriteLine($"{(int)comicType}. {comicType}");
            }

            Comic.ComicType selectedType;
            while (true)
            {
                Console.WriteLine("Selecciona el tipo de cómic (número):");
                if (int.TryParse(Console.ReadLine(), out int comicTypeSelection) && comicTypeSelection > 0 && comicTypeSelection <= Enum.GetValues(typeof(Comic.ComicType)).Length)
                {
                    selectedType = (Comic.ComicType)comicTypeSelection; // Casting del integer al enum
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Tipo de cómic no válido.");
                }
            }
            
            Comic comic= new Comic(name, author, publisher, yearPublished, price, isRead, isForAdults, selectedGenres, selectedType);
            comic.Genres = selectedGenres;
            comic.ShowComicInformation();

            foreach (var genre in selectedGenres)
            {
                genre.Comics.Add(comic.Name);
            }

            if (comics == null)
            {
                Console.WriteLine("La lista de cómics no está inicializada.");
                return;
            }

            comics.Add(comic);
            JsonUtils.SaveDataToJson(comics, Constants.ComicsFileName);
            JsonUtils.SaveDataToJson(GenreService.genres, Constants.GenresFileName);
                
        }
        catch (InvalidComicException ex) 
        {
            var messageError = "InvalidComicException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
        }
    }      




    public static void ShowAllComics()
    {
        Console.WriteLine("\nListado de Cómics:");
        foreach (var comic in comics)
        {
            if (comic != null)
            {
                comic.ShowComicInformation();
            }
            else
            {
                Console.WriteLine("Cósmico nulo encontrado en la lista.");
            }
        }
    }


    
    public static void SearchComic()
    {
        try 
        {
            Console.WriteLine("Introduce el nombre de cómic:");
            string name = Console.ReadLine();
            Comic comic = comics.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (comic != null)
            {
                comic.ShowComicInformation();
            }
            else
            {
                Console.WriteLine("Cómic no encontrado.");
            }
        }
        catch (InvalidComicException ex) 
        {
            var messageError = "InvalidComicException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
        }
    }




    public static void DeleteComic()
    {
        ShowAllComics();
    
        try
        {
            Console.WriteLine("Selecciona el ID del comic a eliminar:");

            if (int.TryParse(Console.ReadLine(), out int IdSelected))
            {
                Comic comic = comics.Find(c => c.Id.Equals(IdSelected));
                if (comic != null){
                    comics.Remove(comic);
                    Console.WriteLine("Cómic eliminado correctamente");
                    ShowAllComics();
                    JsonUtils.SaveDataToJson(comics, Constants.ComicsFileName);
                    JsonUtils.SaveDataToJson(GenreService.genres, Constants.GenresFileName);

                }
                else{
                    Console.WriteLine("No hay ningún cómic con el ID introducido");
                }
            }   
        }
        catch(InvalidComicException ex)
        {
            var messageError = "InvalidComicException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
        }
    }

}