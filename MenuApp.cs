namespace Models; 

class MenuApp
{

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
                    break;
                case 3:
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
}