using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public bool IsDeleted { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } /// com o VIRTUAL o entity consegue sobrescrever a prop de navegação
    }
}
