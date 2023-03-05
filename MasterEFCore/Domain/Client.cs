using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Domain
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public Adress Adress { get; set; }
    }

    public class Adress
    {
        public string CEP { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
