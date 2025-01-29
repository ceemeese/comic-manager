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
    public DateTime? DateCreated { get; private set; }
    public string Telephone { get; set; }
    public bool IsAdmin { get; set; } = false;
    public List<Comic> PersonalComics { get; set; }
    



    //Constructor
    public User(string name, string mail, string password, string telephone, DateTime? dateCreated = null ) 
    {
        Id = nextId;
        nextId++;
        Name = name;
        Mail = mail;
        Password = password;
        DateCreated = dateCreated ?? DateTime.Now;
        Telephone = telephone;
        IsAdmin = false;
        PersonalComics = new List<Comic>();
    }


    //Mostrar información
    public void ShowUserInformation() 
    {
        Console.WriteLine($"ID: {Id}, Nombre: {Name}, Correo: {Mail}, Teléfono: {Telephone}, Fecha Alta: {DateCreated:g} ");

        if (PersonalComics != null && PersonalComics.Count != 0) 
        {
            Console.WriteLine("Cómics:");
            foreach (var comic in PersonalComics) 
            {
                Console.WriteLine($"Título: {comic}, Autor: {comic}");
            }
        }
        else{
            Console.WriteLine("Este usuario no tiene cómics registrados");
        }
    }

}
