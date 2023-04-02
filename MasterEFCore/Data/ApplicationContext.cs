using MasterEFCore.Configurations;
using MasterEFCore.Conversores;
using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MasterEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        #region MODULO INICIAL ATÉ DATAANOTATIONS
        
        //private readonly StreamWriter _writer = new StreamWriter(@"C:\Users\wesll\source\desenvolvedor-io\master-ef-core\MasterEFCore\meu_log_do_ef_core.txt", append: true);
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Estate> Estates { get; set; }
        public DbSet<Conversor> Conversores { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");
        public DbSet<Atributo> Atributos { get; set; }
        public DbSet<Aeroporto> Aeroportos { get; set; }
        public DbSet<Voo> Voos { get; set; }
       
        #endregion

        public DbSet<Funcao> Funcoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=MasterEFCore; Integrated Security=True;pooling=True";

            optionsBuilder
                .UseSqlServer(strConnection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region MODULO MODELO DE DADOS ATÉ EF FUNCTIOS
            /*
              *COLLATIONS* 
            //SQL_Latin1_General:regras básicas de agrupamento utilizado pelo o windowns.
            //CP1: codificação ANSI 1252 que é utilizada no windons.
            //CI: Especifica que a collation é Case Insensitive ou CS:Case Sensitive.
            //AI: Especifica Acentuação insensitive ou AS: Acentuação Sensitive.
            //Definindo Colletion Global
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");
            //Definindo Colletion Especifica.
            modelBuilder.Entity<Department>().Property(p => p.Description).UseCollation("SQL_Latin1_General_CP1_CS_AS");
            */

            /*
             *SEQUENCES*
            modelBuilder
                .HasSequence<int>("MinhaSequencia", "sequencia")
                .StartsAt(1) //inicia em 1
                .IncrementsBy(2) //increment em em 2
                .HasMin(1) //valor min da sequencia
                .HasMax(10) //valor max da sequencia
                .IsCyclic(); //qnd cehagr ao máximo, reinicia para o valor mínimo definido.

            modelBuilder.Entity<Department>().Property(p => p.Id).HasDefaultValueSql("NEXT VALUE FOR sequencia.MinhaSequencia");
            */

            /*
            *INDICES*
            modelBuilder
                .Entity<Department>()
                //.HasIndex(d => d.Description); //indice simples
                .HasIndex(d => new { d.Description, d.Active }) //indice composto
                .HasDatabaseName("idx_meu_indice_composto")
                .HasFilter("Description IS NOT NULL") //Filtro para criar indice
                .HasFillFactor(80) //Fator de preenchimento, armazenamento max de uma pág no sql é de 8k
                .IsUnique(); 
            */

            /*
             * PROPAGACAO DE DADOS
            //HasData habilita Indentity insert, onde posso passar valor pra minha chave primaria mesmo a mesma sendo auto increment.
            modelBuilder.Entity<Estate>().HasData(new[]
            {
                new Estate { Id= 1, Name="São Paulo"},
                new Estate { Id= 2, Name="Sergipe"},
            });
            */

            /*
             * CONFIGURANDO ESQUEMAS
            modelBuilder.HasDefaultSchema("cadastros");
            modelBuilder.Entity<Estate>().ToTable("Estates", "SegundoEsquema");
            */


            /*
             * CONVERSION
            var conversao = new ValueConverter<Versao, string>(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p));
            //Microsoft.EntityFrameworkCore.Storage.ValueConversion.MetodoDeConversao, local com vários conversores já definido.
            var conversao1 = new EnumToStringConverter<Versao>();
            modelBuilder
                .Entity<Conversor>()
                .Property(x => x.Versao)
                .HasConversion(conversao1); //expressao externa utilizando um converter já definido.
                //.HasConversion(conversao); //expressão externa customizada.
                //.HasConversion(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p)); //expressão inline
                //.HasConversion<string>(); //forma de conversão para salvar no banco simples enum em string.
            /*

            /*
              * UTILIZANDO CONVERSOR CUSTOMIZADO
            modelBuilder
               .Entity<Conversor>()
               .Property(x => x.Status)
               .HasConversion(new ConversorCustomizado());
            /*

            /*
              * CONFIG PROPRIEDADE DE SOMBRA
            modelBuilder.Entity<Department>().Property<DateTime>("LastUpdate");
            */


            /*MANEIRAS DE CHAMAR CONFIGURAÇÕES DA ENTIDADE EM OUTRO ARQUIVO */
            //modelBuilder.ApplyConfiguration(new ClientConfiguration()); //chamando arquivo separado um a um. Para cada config gerar uma linha dessa.
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //chamando todas as configuration que foram implementadas no assembly. forma 1
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly); //chamando todas as configuration que fora implementadas no assembly. forma 2

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", p =>
            {
                p.Property<int>("Id");

                p.Property<string>("Chave")
                .HasColumnType("VARCHAR(40)")
                .IsRequired();

                p.Property<string>("Valor")
               .HasColumnType("VARCHAR(40)")
               .IsRequired();
            });

            //EX Modulo EF FUNCTIONs
            modelBuilder.Entity<Funcao>(conf =>
            {
                conf.Property<string>("PropriedadeSombra")
                .HasColumnType("VARCHAR(100)")
                .HasDefaultValueSql("'Teste'");
            });

            #endregion
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

        #region MODULO INFRAESTRUTURA
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    const string strConnection = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=MasterEFCore; Integrated Security=True;pooling=True";

        //    optionsBuilder
        //        .UseSqlServer(
        //            strConnection,
        //                x => x
        //                .MaxBatchSize(100) //por default batchSize max é 42, se passar de 42, ex:50 o entity vai no banco 2x. não aconselhavel aumentar o batchsize se a internet for instavel.
        //                .CommandTimeout(5) //configurando timeout global. Default é 30seg
        //                .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null)) //Resiliencia conexão, config default ao habilitar ele tentará 6x por 30seg até falhar. 
        //        .LogTo(Console.WriteLine, LogLevel.Information)
        //        .EnableSensitiveDataLogging(); //prop para ver dados sensiveis, ex: parametros enviados na consulta. aconselhavel somente em modo debug
        //        //.LogTo(Console.WriteLine, new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted },
        //        //LogLevel.Information, DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine);
        //        //.LogTo(_writer.WriteLine, LogLevel.Information);
        //        //.EnableDetailedErrors(); //prop para detalhar erros das entidades e descobrir erros na aplicação. aconselhavel somente em modo debug
        //}

        //public override void Dispose()
        //{
        //    base.Dispose();
        //    _writer.Dispose();
        //}
        #endregion
    }
}
