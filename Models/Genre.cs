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
    private static int nextId = 1;

    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }  
    public int Priority{ get; set; }
    public string Icon { get; set; }
    public List<string> Comics { get; set; }
    public DateTime? DateCreated { get; private set; }

    public override string ToString()
    {
        return Name;
    }
    

    public Genre (string name, string description, int priority, string icon, DateTime? dateCreated = null) {
        Id = nextId;
        nextId++;
        Name = name;
        Description = description;
        Priority = priority;
        Icon = icon;
        Comics = Comics = new List<string>();
        DateCreated = dateCreated ?? DateTime.Now;
        
    }

    public void ShowGenreInformation()
    {
        Console.WriteLine($"ID: {Id}, Nombre: {Name}, Description: {Description}, Icono: {Icon}, Fecha Creación: {DateCreated:g}");

        if (Comics != null && Comics.Any()) 
        {
            Console.WriteLine("Cómics:");
            foreach (var comic in Comics) 
            {
                Console.WriteLine($"- {comic}");
            }
        }
        else{
            Console.WriteLine("No hay cómics en este género");
        }
    }
}

