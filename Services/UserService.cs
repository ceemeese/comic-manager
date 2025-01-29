namespace Services;

using Models;
using Utils;

class UserService
{
    public static List<User> users = JsonUtils.LoadDataJson<User>(Constants.UsersFileName) ?? new List<User>();
    public static User? currentUser = null;
    //public static List<User> users = new List<User>();


    public UserService()
    {

    }



    public static void AddUser()
    {

        try
        {
            Console.WriteLine("___NUEVO USUARIO___");
            Console.WriteLine("Nombre: ");
            string name = Console.ReadLine();

            //Validacion mail
            string mail;
            while(true)
            {
                Console.WriteLine("Correo: ");
                mail = Console.ReadLine();
                if (ValidationUtils.IsValidMail(mail))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: El correo no es válido.");
                }
            }

            if (users.Any(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase)))
            {
            throw new InvalidComicException("Error: Ya existe un usuario con este mail");
            }


            string password;
            while(true)
            {
                Console.WriteLine("Contraseña (Debe contener mínimo un número, una mayúscula y mínimo 8 carácteres): ");
                password = Console.ReadLine();
                
                if (ValidationUtils.IsValidPassword(password))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: La contraseña no és válida.");
                }
            }

            Console.WriteLine("Teléfono: ");
            string telephone = Console.ReadLine();

            bool admin = false;
            if (currentUser != null && currentUser.IsAdmin)
            {
                Console.WriteLine("Es administrador? (si/no): ");
                string answer = Console.ReadLine();
                admin = answer.ToLower() == "yes" ? true : false;
            }


            User user = new User(name, mail, password, telephone, admin);
            Console.WriteLine("Usuario registrado correctamente");
            user.ShowUserInformation();
            users.Add(user);
            JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
        } 
        catch (InvalidUserException ex) 
        {
            var messageError = "InvalidUserException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
            Console.WriteLine(messageError);
        }
    }


    public static void ShowAllUsers()
    {
        Console.WriteLine("\nListado de Usuarios:");
        foreach (var user in users)
        {
            user.ShowUserInformation();
        }
    }



    public static void SearchUser()
    {
        try 
        {
            Console.WriteLine("Introduce el correo del usuario:");
            string mail = Console.ReadLine();
            User user = users.Find(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                user.ShowUserInformation();
            }
            else
            {
                throw new InvalidComicException("Error: Usuario no existe");
            }
        }
        catch (InvalidUserException ex) 
        {
            var messageError = "InvalidUserException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch (Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
            Console.WriteLine(messageError);
        }
    }



    public static void DeleteUser()
    {
        ShowAllUsers();
    
        try
        {
            Console.WriteLine("Selecciona el ID del usuario a eliminar:");

            if (int.TryParse(Console.ReadLine(), out int IdSelected))
            {
                User user = users.Find(u => u.Id.Equals(IdSelected));
                if (user != null){
                    users.Remove(user);
                    Console.WriteLine("Usuario eliminado correctamente");
                    ShowAllUsers();
                    JsonUtils.SaveDataToJson(users, Constants.UsersFileName);
                }
                else{
                    throw new InvalidComicException("Error: No hay ningún usuario con ese ID");
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
            Console.WriteLine(messageError);
        }
    }


    public static void ManageComicsInUserList(User loggedUser, bool isAddOperation) 
    {
        List<Comic> selectedComics = new List<Comic>();
        string operation = isAddOperation ? "añadir" : "eliminar";

        while(true)
        {
            Console.WriteLine(isAddOperation ? "Cómics disponibles: " : "Tus cómics personales: ");
            List<Comic> comicsList = isAddOperation ? ComicService.comics : loggedUser.PersonalComics;
            
            for (int i = 0; i < comicsList.Count; i++)
            {
                Console.WriteLine($"{ComicService.comics[i].Id}. {ComicService.comics[i].Name}");
            }

            Console.WriteLine($"Introduce los números de los comics que quieres {operation} separados por comas (ejemplo: 1,3,5):");
            string answer = Console.ReadLine();


            if (string.IsNullOrWhiteSpace(answer))
            {
                Console.WriteLine("Error: No has seleccionado ningún cómic. Intenta de nuevo.");
                continue;
            }

            string[] comicIndexArray = answer.Split(',');
            selectedComics.Clear();

            bool isValid = true;

            foreach (string index in comicIndexArray)
            {
                if (int.TryParse(index.Trim(), out int comicId) && comicId > 0 && ComicService.comics.Any(c => c.Id == comicId))
                {
                    Comic comic = ComicService.comics.FirstOrDefault(c => c.Id == comicId);

                    if (isAddOperation && loggedUser.PersonalComics.Any(c => c.Id == comic.Id))
                    {
                        Console.WriteLine($"El cómic '{comic.Name}' ya está en tu lista personal.");
                    }
                    else
                    {
                        selectedComics.Add(comic);
                    }
                }
                else
                {
                    Console.WriteLine($"Error: La opción '{index}' marcada no es correcta. Intenta de nuevo.");
                    isValid = false;
                    break;
                }
            }

            if (isValid && selectedComics.Count > 0)
            {
                break;
            }
        }

        foreach (var comic in selectedComics)
        {
            if (isAddOperation)
            {
                loggedUser.PersonalComics.Add(comic);
                Console.WriteLine($"El cómic '{comic.Name}' se ha añadido a tu lista personal.");
            }
            else
            {
                loggedUser.PersonalComics.Remove(comic);
                Console.WriteLine($"El cómic '{comic.Name}' ha sido eliminado de tu lista personal.");
            }
        }

    }



    // Mostrar los cómics del usuario
    public static void ShowUserComics(User user)
    {
        Console.WriteLine($"Hola {user.Name}, esta es tu lista de cómics:");

        if (user.PersonalComics.Count > 0)
        {
            foreach (var comic in user.PersonalComics)
            {
                Console.WriteLine(comic.Name);
            }
        }
        else
        {
            Console.WriteLine("No tienes cómics registrados.");
        }
    }




    public static void Login()
    {
        Console.WriteLine("__LOGIN__");
        Console.WriteLine("Mail: ");
        string mail = Console.ReadLine();
        Console.WriteLine("Password: ");
        string password = Console.ReadLine();

        User? user = users.FirstOrDefault(u => u.Mail.Equals(mail, StringComparison.OrdinalIgnoreCase) && u.Password == password);

        if (user != null)
        {
            currentUser = user;
            Console.WriteLine($"Hola, {user.Name}!");
            return;
        }
        Console.WriteLine("Error: Nombre de usuario o contraseña incorrectos.");

    }


    public static void Logout()
    {
        if (currentUser != null)
        {
            Console.WriteLine($"Hasta pronto, {currentUser.Name}!");
            currentUser = null;
        }
        else
        {
            Console.WriteLine("No hay usuario conectado.");
        }
    }

}