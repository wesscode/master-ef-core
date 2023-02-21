using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// com o virtual o entity consegue sobrescrever a prop de navegação
        /// </summary>
        public virtual List<Employee> EmployeeList { get; set; }
    }
}
