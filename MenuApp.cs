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
            
            if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
            {
                Console.WriteLine("3. Usuarios");
            }

            if (UserService.currentUser != null)
            {
                Console.WriteLine("4. Zona privada");
                Console.WriteLine("5. Cerrar sesión");
                Console.WriteLine("6. Salir");
            }
            else
            {
                Console.WriteLine("4. Iniciar sesión");
                Console.WriteLine("5. Registrarse");
                Console.WriteLine("6. Salir");
            }

            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 6)
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
                    if (UserService.currentUser != null)
                        ShowPrivateMenu();
                    else
                        UserService.Login();
                    break;
                case 5:
                    if (UserService.currentUser != null)
                        UserService.Logout();
                    else
                        UserService.AddUser();
                    break;
                case 6:
                    Console.WriteLine("¡Hasta pronto!");
                    break;
                default:
                    Console.WriteLine("La opción no es correcta");
                    break;
            }
        }

        while(option != 6);
    }




    private void ShowGenreMenu()
    {
        int option = 0;

        do
        {
            Console.WriteLine("\n--- MENÚ GÉNEROS ---");

            if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
            {
                Console.WriteLine("1. Añadir género");
            }
            Console.WriteLine("2. Listar géneros");
            Console.WriteLine("3. Buscar género");

            if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
            {
                Console.WriteLine("4. Eliminar género");
            }

            Console.WriteLine("5. Volver al menú principal");
            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse((string)Console.ReadLine(), out option) || option < 1 || option > 5)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida");
                    continue;
            }

            switch (option)
            {
                case 1:
                    if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
                        GenreService.AddGenre();
                    else
                        Console.WriteLine("Error: Por favor selecciona una opción válida");
                    break;
                case 2:
                    GenreService.ShowAllGenres();
                    break;
                case 3:
                    GenreService.SearchGenre();
                    break;
                case 4:
                    if (UserService.currentUser != null && UserService.currentUser.IsAdmin)
                        GenreService.DeleteGenre();
                    else
                        Console.WriteLine("Error: Por favor selecciona una opción válida");
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
            Console.WriteLine("\n--- MENÚ COMICS ---");
            Console.WriteLine("1. Añadir cómic");
            Console.WriteLine("2. Listar cómics");
            Console.WriteLine("3. Buscar cómic");
            Console.WriteLine("4. Eliminar cómic");
            Console.WriteLine("5. Volver al menú principal");
            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse((string)Console.ReadLine(), out option) || option < 1 || option > 5)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-5)");
                    continue;
            }

            switch (option)
            {
                case 1:
                    if (UserService.currentUser != null)
                        ComicService.AddComic();
                    else
                        Console.WriteLine("Debes iniciar sesión para realizar esta acción.");
                    break;
                case 2:
                    ComicService.ShowAllComics();
                    break;
                case 3:
                    ComicService.SearchComic();
                    break;
                case 4:
                    if (UserService.currentUser != null)
                        ComicService.DeleteComic();
                    else
                        Console.WriteLine("Debes iniciar sesión para realizar esta acción.");
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



    private void ShowPrivateMenu()
    {
        int option = 0;

        do
        {
            Console.WriteLine("\n--- ZONA PRIVADA ---");
            Console.WriteLine("1. Añadir cómic a mi lista personal");
            Console.WriteLine("2. Eliminar cómic a mi lista personal");
            Console.WriteLine("3. Ver mi lista personal de cómics");
            Console.WriteLine("4. Volver al menú principal");

            Console.WriteLine("Selecciona una opción:");

            if (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 4)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-4).");
                continue;
            }

            switch (option)
            {
                case 1:
                    UserService.ManageComicsInUserList(UserService.currentUser, true);
                    break;
                case 2:
                    UserService.ManageComicsInUserList(UserService.currentUser, false);
                    break;
                case 3:
                    UserService.ShowUserComics(UserService.currentUser);
                    break;
                case 4:
                    Console.WriteLine("Volviendo...");
                    break;
                default:
                    Console.WriteLine("La opción no es correcta");
                    break;
            }
        } while (option != 4);
    }
}