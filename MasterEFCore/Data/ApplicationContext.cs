using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MasterEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly StreamWriter _writer = new StreamWriter(@"C:\Users\wesll\source\desenvolvedor-io\master-ef-core\MasterEFCore\meu_log_do_ef_core.txt", append: true);
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=MasterEFCore; Integrated Security=True;pooling=True";

            optionsBuilder
                .UseSqlServer(
                    strConnection,
                        x => x
                        .MaxBatchSize(100) //por default batchSize max é 42, se passar de 42, ex:50 o entity vai no banco 2x. não aconselhavel aumentar o batchsize se a internet for instavel.
                        .CommandTimeout(5) //configurando timeout global. Default é 30seg
                        .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null)) //Resiliencia conexão, config default ao habilitar ele tentará 6x por 30seg até falhar. 
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging(); //prop para ver dados sensiveis, ex: parametros enviados na consulta. aconselhavel somente em modo debug
                //.LogTo(Console.WriteLine, new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted },
                //LogLevel.Information, DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine);
                //.LogTo(_writer.WriteLine, LogLevel.Information);
                //.EnableDetailedErrors(); //prop para detalhar erros das entidades e descobrir erros na aplicação. aconselhavel somente em modo debug
        }

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }

        #region MODULO INICIAL ATE PROCEDURES
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    const string strConnection = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=MasterEFCore; Integrated Security=True;pooling=True"; //MultipleActiveResultSets=true (habilita mais de uma conexao aberta com o banco, outra opção é utilizar .ToList() pra fechar a consulta.)
        //    optionsBuilder
        //        //.UseSqlServer(strConnection, p => p.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)) //split query global
        //        .UseSqlServer(strConnection)
        //        .EnableSensitiveDataLogging()
        //        //.UseLazyLoadingProxies()
        //        .LogTo(Console.WriteLine, LogLevel.Information);
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Department>().HasQueryFilter(p => !p.IsDeleted);
        //}
        #endregion
    }
}
