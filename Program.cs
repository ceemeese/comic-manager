using Models;

List<Genre> genres = new List<Genre>();

var user = new User("Cris", "cris@sanvalero.com", "pass", "999999999" );
user.ShowUserInformation();


var genre = new Genre( "Superheroes", "Descripción superheroes", 1, "#1234");
genre.ShowGenreInformation();

var menuApp = new MenuApp();
menuApp.ShowMenu();