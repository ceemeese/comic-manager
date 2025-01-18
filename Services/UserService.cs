namespace Services;
using Models;
using Utils;


class UserService
{


    public UserService()
    {

    }



    public static void userRegister()
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
}