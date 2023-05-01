using EFCore.Tips.Domain;
using EFCore.Tips.Extensions;
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
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<UsuarioFuncao> UsuarioFuncoes { get; set; }
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

            //GetEntityTypes: obtendo tipos das entidades.
            //SelectMany(e => e.GetProperties(): selecionando as properties
            //Where(p => p.ClrType == typeof(string): onde tipo da prop igual string
            //&& p.GetColumnType() == null: e tipo da coluna não definido ainda.
            var properties = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetProperties())
                .Where(p => p.ClrType == typeof(string) && p.GetColumnType() == null);
            foreach (var property in properties)
            {
                property.SetIsUnicode(false); //retiro padrão unicode por default.
            }

            //invocando o metodo de extensão
            modelBuilder.ToSnakeCaseNames();
        }
    }
}
