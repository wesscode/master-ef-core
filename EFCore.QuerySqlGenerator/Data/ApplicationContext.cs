using EFCore.QuerySqlGenerator.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EFCore.QuerySqlGenerator.MySqlServerQuerySqlGenerator;

namespace EFCore.QuerySqlGenerator.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Data Source=(localdb)\\mssqllocaldb; Database=SobrescrevendoComportamento; Integrated Security=true")
                //.ReplaceService<IQuerySqlGeneratorFactory, MySqlServerQuerySqlGeneratorFactory>() //fazer replace do serviço, informando o efcore que ele precisa utilizar esse serviço customizado, quando passar pelo o processamento de pipeline de criaçao de query.
                //.LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();
        }
    }
}
