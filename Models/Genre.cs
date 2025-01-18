namespace Models;

class InvalidGenreException: Exception 
{
    public InvalidGenreException(string message = ""):base(message) 
    {

    }
}

class Genre
{
    //Variable para autoincremento
    private static int nextId = 0;

    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }  
    public int Priority{ get; set; }
    public string Icon { get; set; }
    public List<Comic> Comics { get; set; } = new List<Comic>();
    

    public Genre (string name, string description, int priority, string icon) {
        Id = nextId++;
        Name = name;
        Description = description;
        Priority = priority;
        Icon = icon;
    }

    public void ShowGenreInformation()
    {
        Console.WriteLine($"ID: {Id}, Nombre: {Name}, Description: {Description}, Icono: {Icon},");

        if (Comics != null && Comics.Any()) 
        {
            Console.WriteLine("Cómics:");
            foreach (var comic in Comics) 
            {
                Console.WriteLine($"Título: {comic.Name}, Autor: {comic.Author}");
            }
        }
        else{
            Console.WriteLine("No hay cómics en este género");
        }
    }
}

