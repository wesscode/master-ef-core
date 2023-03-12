using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Domain
{
    public class Estate
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Governador Governador { get; set; }
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
}
