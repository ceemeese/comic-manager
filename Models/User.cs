namespace Models;

class InvalidUserException: Exception 
{
    public InvalidUserException(string message = ""):base(message) 
    {

    }
}

class User
{
    private static int nextId = 1;
    public int Id { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public DateTime DateCreated { get; private set; }
    public string Telephone { get; set; }
    public bool IsAdmin { get; set; } = false;
    public List<string> Comics { get; set; }
    



    //Constructor
    public User(string name, string mail, string password, string telephone) 
    {
        Id = nextId;
        nextId++;
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
        Console.WriteLine($"ID: {Id}, Nombre: {Name}, Correo: {Mail}, Teléfono: {Telephone}, Fecha Alta: {DateCreated:g} ");

        if (Comics != null && Comics.Any()) 
        {
            Console.WriteLine("Cómics:");
            foreach (var comic in Comics) 
            {
                Console.WriteLine($"Título: {comic}, Autor: {comic}");
            }
        }
        else{
            Console.WriteLine("Este usuario no tiene cómics registrados");
        }
    }


}
