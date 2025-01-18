namespace Models; 
using Models;
using Services;

class MenuApp
{

    public MenuApp()
    {
        
    }

    public void ShowMenu()
    {
        
        int option = 0;
        
        do
        {
            Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
            Console.WriteLine("1. Géneros");
            Console.WriteLine("2. Comics");
            Console.WriteLine("3. Usuarios");
            Console.WriteLine("4. Salir");
            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 4)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-6).");
                continue;
            }

            switch (option)
            {
                case 1:
                    showGenreMenu();
                    break;
                case 2:
                    showComicMenu();
                    break;
                case 3:
                    showUserMenu();
                    break;
                case 4:
                    Console.WriteLine("¡Hasta pronto!");
                    break;
                default:
                    Console.WriteLine("La opción no es correcta");
                    break;
            }
        }

        while(option != 4);
    }




    private void showGenreMenu()
    {
        int option = 0;

        do
        {
            Console.WriteLine("\n--- MENÚ GÉNEROS ---");
            Console.WriteLine("1. Añadir género");
            Console.WriteLine("2. Listar géneros");
            Console.WriteLine("3. Buscar género");
            Console.WriteLine("4. Eliminar género");
            Console.WriteLine("5. Volver al menú principal");
            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse((string)Console.ReadLine(), out option) || option < 1 || option > 5)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-5).");
                    continue;
            }

            switch (option)
            {
                case 1:
                    GenreService.AddGenre();
                    break;
                case 2:
                    GenreService.ShowAllGenres();
                    break;
                case 3:
                    GenreService.SearchGenre();
                    break;
                case 4:
                    break;
                case 5:
                    Console.WriteLine("Volviendo..");
                    break;
                default:
                    Console.WriteLine("La opción no es correcta");
                    break;
            }

        }
        while (option != 5);

    }



    private void showComicMenu()
    {
        int option = 0;

        do
        {
            Console.WriteLine("\n--- MENÚ CÓMIC ---");
            Console.WriteLine("1. Añadir cómic");
            Console.WriteLine("2. Listar cómics");
            Console.WriteLine("3. Buscar cómic");
            Console.WriteLine("4. Eliminar cómic");
            Console.WriteLine("5. Volver al menú principal");
            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse((string)Console.ReadLine(), out option) || option < 1 || option > 5)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-5).");
                    continue;
            }

            switch (option)
            {
                case 1:
                    ComicService.addComic();
                    break;
                case 2:
                    ComicService.showAllComics();
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    Console.WriteLine("Volviendo..");
                    break;
                default:
                    Console.WriteLine("La opción no es correcta");
                    break;
            }

        }
        while (option != 5);

    }




    private void showUserMenu()
    {
        int option = 0;

        do
        {
            Console.WriteLine("\n--- MENÚ DE USUARIOS ---");
            Console.WriteLine("1. Añadir usuario");
            Console.WriteLine("2. Listar usuarios");
            Console.WriteLine("3. Buscar usuario");
            Console.WriteLine("4. Eliminar usuario");
            Console.WriteLine("5. Volver al menú principal");
            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse((string)Console.ReadLine(), out option) || option < 1 || option > 5)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-5).");
                    continue;
            }

            switch (option)
            {
                case 1:
                    UserService.addUser();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    Console.WriteLine("Volviendo..");
                    break;
                default:
                    Console.WriteLine("La opción no es correcta");
                    break;
            }

        }
        while (option != 5);

    }
}