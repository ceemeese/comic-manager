namespace Models;

class InvalidComicException: Exception 
{
    public InvalidComicException(string message = ""):base(message) 
    {

    }
}

class Comic 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public int YearPublished { get; set; }
    public decimal Price { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsForAdults { get; set; } = true;
    public List<Genre> Genres { get; set; }
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

        Console.WriteLine($"Nombre: {Name}, Autor: {Author}, Año: {YearPublished}, Precio: {Price}, Leído: {read}, Es para adultos?: {adults}");
        if (Genres.Any())
        {
            Console.WriteLine("Géneros:");
            foreach (var genre in Genres)
            {
                Console.WriteLine($"- {genre.Name}");
            }
        }
    }

}