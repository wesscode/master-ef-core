using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Instructor : Person
    {
        public DateTime Desde { get; set; }
        public string Tecnology { get; set; }
    }
    public class Student : Person
    {
        public int Age { get; set; }
        public DateTime ContractDate { get; set; }
    }
}
