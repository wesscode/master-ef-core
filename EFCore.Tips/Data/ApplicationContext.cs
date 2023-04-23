using EFCore.Tips.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Tips.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Departamento> Departamentos{ get; set; }
        public DbSet<UsuarioFuncao> UsuarioFuncoes{ get; set; }
        public DbSet<DepartamentoRelatorio> DepartamentoRelatorio { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Data Source=(localdb)\\mssqllocaldb; Database=tips; Integrated Security=true")
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //equivalente ao dataAnnotation KeyLess
            //modelBuilder.Entity<UsuarioFuncao>().HasNoKey();

            modelBuilder.Entity<DepartamentoRelatorio>(e =>
            {
                e.HasNoKey();

                e.ToView("vw_departamento_relatorio"); //essa anotação avisamos ao efcore que não deve criar tabela e sim consumir uma view que já existe.

                e.Property(p => p.Departamento).HasColumnName("Descricao");
            });
        }
    }
}
