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
                    ShowGenreMenu();
                    break;
                case 2:
                    ShowComicMenu();
                    break;
                case 3:
                    ShowUserMenu();
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




    private void ShowGenreMenu()
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
                    GenreService.DeleteGenre();
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



    private void ShowComicMenu()
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
                    ComicService.AddComic();
                    break;
                case 2:
                    ComicService.ShowAllComics();
                    break;
                case 3:
                    ComicService.SearchComic();
                    break;
                case 4:
                    ComicService.DeleteComic();
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




    private void ShowUserMenu()
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
                    UserService.AddUser();
                    break;
                case 2:
                    UserService.ShowAllUsers();
                    break;
                case 3:
                    UserService.SearchUser();
                    break;
                case 4:
                    UserService.DeleteUser();
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