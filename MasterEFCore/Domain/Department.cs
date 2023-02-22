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
        public bool IsDeleted { get; set; }

        public List<Employee> EmployeeList { get; set; }

        public Department()
        {
        }

        ////Lazy loading sem proxies Com Action
        //private Action<object, string> _lazyLoader { get; set; }
        //public Department(Action<object, string> lazyLoader)
        //{
        //    _lazyLoader = lazyLoader;
        //}

        //private List<Employee> _employeeList;
        //public List<Employee> EmployeeList
        //{
        //    get
        //    {
        //        _lazyLoader?.Invoke(this, nameof(EmployeeList));
        //        return _employeeList;
        //    }
        //    set => _employeeList = value;
        //}


        //lazy Loading sem proxies
        //private ILazyLoader _lazyLoader { get; set; }
        //public Department(ILazyLoader lazyLoader)
        //{
        //    _lazyLoader = lazyLoader;
        //}

        //private List<Employee> _employeeList;
        //public List<Employee> EmployeeList
        //{
        //    get => _lazyLoader.Load(this, ref _employeeList);
        //    set => _employeeList = value;
        //}
    }
}
