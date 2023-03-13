namespace MasterEFCore.Domain
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Movie> MovieList { get; } = new List<Movie>();
    }
    public class Movie
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<Actor> ActorList { get; } = new List<Actor>();
    }
}
