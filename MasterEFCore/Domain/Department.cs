using Microsoft.EntityFrameworkCore.Infrastructure;
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

        public Department()
        {
        }

        private ILazyLoader _lazyLoader { get; set; }
        public Department(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private List<Employee> _employeeList;
        public List<Employee> EmployeeList
        {
            get => _lazyLoader.Load(this, ref _employeeList);
            set => _employeeList = value;
        }
    }
}
