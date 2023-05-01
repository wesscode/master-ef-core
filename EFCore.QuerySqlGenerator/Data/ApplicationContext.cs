using EFCore.QuerySqlGenerator.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.QuerySqlGenerator.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Data Source=(localdb)\\mssqllocaldb; Database=SobrescrevendoComportamento; Integrated Security=true")
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();
        }
    }
}
