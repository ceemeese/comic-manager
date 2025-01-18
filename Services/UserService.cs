namespace Services;
using Models;
using Utils;


class UserService
{
    public static List<User> users = new List<User>();


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

            User user = new User(name, mail, password, telephone);
            Console.WriteLine("Usuario registrado correctamente");
            user.ShowUserInformation();
            users.Add(user);
        } 
        catch (InvalidUserException ex) 
        {
            var messageError = "InvalidUserException:" + ex.Message;
            Console.WriteLine(messageError);
        }
        catch(Exception ex)
        {
            var messageError = "ExceptionError:" + ex.Message;
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
                Console.WriteLine("Usuario no encontrado.");
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
        }
    }
}