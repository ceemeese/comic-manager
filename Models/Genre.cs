namespace Models;

class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";  
    public int Priority{ get; set; }
    public string Icon { get; set; }

    //TODO LISTA COMICS
    

    public Genre (string name, string description, int priority, string icon) {
        Name = name;
        Description = description;
        Priority = priority;
        Icon = icon;
    }

    public void ShowGenreInformation()
    {
        Console.WriteLine($"Nombre: {Name}, Description: {Description}, Icono: {Icon} ");
    }
}

