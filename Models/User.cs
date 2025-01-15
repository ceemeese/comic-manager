namespace Models;

class User
{
    public string Name { get; set; }
    public string Mail { get; set; }
    public DateTime DateCreated { get; private set; }
    public string Telephone { get; set; }
    public bool IsAdmin { get; set; } = false;
    public List<Comic> Comics { get; set; }



    //Constructor
    public User(string name, string mail, string telephone) 
    {
        Name = name;
        Mail = mail;
        DateCreated = DateTime.Now;
        Telephone = telephone;
        IsAdmin = false;
    }
}
