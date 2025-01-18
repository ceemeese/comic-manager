namespace Models;

class InvalidComicException: Exception 
{
    public InvalidComicException(string message = ""):base(message) 
    {

    }
}

class Comic 
{
    //Variable para autoincremento
    private static int nextId = 1;

    public int Id { get; private set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public int YearPublished { get; set; }
    public decimal Price { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsForAdults { get; set; } = true;
    public List<Genre> Genres { get; set; } = new List<Genre>();
    public ComicType Type { get; set; }
    


    public enum ComicType
    {
        Americano = 1,
        Europeo = 2,
        Manga = 3,
        Manhwa = 4,
        Manhua = 5,
        Latinoamericano = 6,
        Webcomic = 7
    }

    //Constructor
    public Comic (string name, string author, string publisher, int yearPublished, decimal price, bool isRead, bool isForAdults, List<Genre> genres, ComicType type)
    {
        Id = nextId;
        nextId++;
        Name = name;
        Author = author;
        Publisher = publisher;
        YearPublished = yearPublished;
        Price = price;
        IsRead = isRead;
        IsForAdults = isForAdults;
        Genres = genres ?? new List<Genre>();
        Type = type;
    }


    public void ShowComicInformation()
    {
        string read = IsRead ? "Sí" : "No";
        string adults = IsForAdults ? "Sí" : "No"; 

        Console.WriteLine($"ID: {Id}, Nombre: {Name}, Autor: {Author}, Año: {YearPublished}, Precio: {Price}, Leído: {read}, Es para adultos?: {adults}, Tipo de Cómic: {Type} ");
        if (Genres != null && Genres.Any()) // Verifica que Genres no sea nulo
        {
            Console.WriteLine("Géneros:");
            foreach (var genre in Genres)
            {
                Console.WriteLine($"- {genre.Name}");
            }
        }
        else
        {
            Console.WriteLine("Este cómic no tiene géneros asociados.");
        }
    }

}