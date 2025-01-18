namespace Models; 
using Models;
using Services;
using System.Text.RegularExpressions;

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
            Console.WriteLine("1. Añadir Genero");
            Console.WriteLine("2. Añadir Comic");
            Console.WriteLine("3. Listar Generos");
            Console.WriteLine("4. Listar Comics");
            Console.WriteLine("5. Añadir Usuario Usuarios");
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
                    GenreService.addGenre();
                    break;
                case 2:
                    ComicService.addComic();
                    break;
                case 3:
                    GenreService.showAllGenres();
                    break;
                case 4:
                    ComicService.showAllComics();
                    break;
                case 5:
                    UserService.userRegister();
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
}