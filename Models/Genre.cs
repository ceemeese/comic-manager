using System.ComponentModel.Design;

namespace Models;

class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";  
    public int Priority{ get; set; }
    public string Icon { get; set; }
    public List<Comic> Comics { get; set; } = new List<Comic>();
    

    public Genre (string name, string description, int priority, string icon) {
        Name = name;
        Description = description;
        Priority = priority;
        Icon = icon;
    }

    public void ShowGenreInformation()
    {
        Console.WriteLine($"Nombre: {Name}, Description: {Description}, Icono: {Icon},");

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

