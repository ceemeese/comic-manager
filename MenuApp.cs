namespace Models; 
using Models;

class MenuApp
{
    private List<Genre> genres;
    private List<Comic> comics;

    public MenuApp()
    {
        genres = new List<Genre>();
        comics = new List<Comic>();
    }

    public void ShowMenu()
    {
        
        int option = 0;
        
        do
        {
            Console.WriteLine("\n--- Menú gestor de cómics ---");
            Console.WriteLine("1. Añadir Género");
            Console.WriteLine("2. Añadir Cómic");
            Console.WriteLine("3. Ver todos los géneros");
            Console.WriteLine("4. Ver todos los cómics");
            Console.WriteLine("5. Registrar usuario");
            Console.WriteLine("6. Salir");
            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 6)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-6).");
                continue;
            }

            switch (option)
            {
                case 1:
                    addGenre();
                    break;
                case 2:
                    addComic();
                    break;
                case 3:
                    showAllGenres();
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    Console.WriteLine("¡Hasta pronto!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("La opción no es correcta");
                    break;
            }
        }
        while(option != 6);
    }



    public void addGenre()
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



    public void addComic() 
    {
        try
        {
            //Control de géneros antes de preguntar. Si no hay géneros, no se pueden añadir cómics
            if (genres.Count == 0)
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
                for (int i = 0; i < genres.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {genres[i].Name}");
                }

                Console.WriteLine("Introduce los números de los géneros separados por comas (ejemplo: 1,3,5):");
                answer = Console.ReadLine();


                if (string.IsNullOrWhiteSpace(answer))
                {
                    Console.WriteLine("Error: No has seleccionado ninguna categoría. Intentelo de nuevo.");
                    continue;
                }

                string[] genreIndexArray = answer.Split(',');
                selectedGenres.Clear();

                bool isValid = true;

                foreach (string index in genreIndexArray)
                {
                    if (int.TryParse(index.Trim(), out int genreIndex) && genreIndex > 0 && genreIndex <= genres.Count)
                    {
                        selectedGenres.Add(genres[genreIndex - 1]);
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
            comic.ShowComicInformation();

            foreach (var genre in selectedGenres)
            {
                genre.Comics.Add(comic);
            }

            comics.Add(comic);
                
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





    public void showAllGenres()
    {
        Console.WriteLine("\nListado de Géneros:");
        foreach (var genre in genres)
        {
            genre.ShowGenreInformation();
        }
    }




}   
