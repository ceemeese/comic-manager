namespace Models;

class User
{
    public string Name { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public DateTime DateCreated { get; private set; }
    public string Telephone { get; set; }
    public bool IsAdmin { get; set; } = false;

    //TODO LISTA COMICS
    



    //Constructor
    public User(string name, string mail, string password, string telephone) 
    {
        Name = name;
        Mail = mail;
        Password = password;
        DateCreated = DateTime.Now;
        Telephone = telephone;
        IsAdmin = false;
    }


    //Mostrar información
    public void ShowUserInformation() 
    {
        Console.WriteLine($"Nombre: {Name}, Correo: {Mail}, Teléfono: {Telephone}, Fecha Alta: {DateCreated} ");
    }


}
