using EFCore.Testes.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Testes.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}
