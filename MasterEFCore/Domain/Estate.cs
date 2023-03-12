
namespace MasterEFCore.Domain
{
    public class Estate
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Governador Governador { get; set; }
        public ICollection<City> Cities { get; } = new List<City>();
    }

    public class Governador
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Partido { get; set; }

        public int EstateId { get; set; }
        public Estate Estate { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int EstateId { get; set; }
        public Estate Estate { get; set; }
    }
}
