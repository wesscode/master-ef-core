// See https://aka.ms/new-console-template for more information
using MasterEFCore.Data;
using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;

static class Program
{
    static void Main(string[] args)
    {
        #region PRIMEIRO MODULO
        //EnsureCreatedAndDelete();

        //GapDoEnsureCreated();

        //HealthCheckDatabase();

        //new ApplicationContext().Departments.AsNoTracking().Any();
        //_count = 0;
        //GerenciarEstadoDaConexao(false);
        //_count = 0;
        //GerenciarEstadoDaConexao(true);

        //SqlInjection();

        //ObterMigracoesPendentes();

        //AplicarMigracaoEmTempoDeExecucao();

        //ObterTodasMigracoes();

        //ObterMigracoesJaAplicadas();

        //ScriptGeralDoBancoDeDados();

        //CarregamentoAdiantado();

        //CarregamentoExplicito();

        //CarregamentoLento();

        #endregion



    }

    #region PRIMEIRO MODULO
    static void EnsureCreatedAndDelete()
    {
        //cria e dropa o banco em tempo de execução.
        using var db = new ApplicationContext();
        //db.Database.EnsureCreated();
        db.Database.EnsureDeleted();
    }

    static void GapDoEnsureCreated()
    {
        //gap ao ter multiplos contextos, ele so entende e executa o primeiro.
        using var db1 = new ApplicationContext();
        using var db2 = new ApplicationContextCity();

        db1.Database.EnsureCreated();
        db2.Database.EnsureCreated();

        //solução para forçar execução do segundo contexto.
        var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
        databaseCreator.CreateTables();
    }

    //checando se estar conectando ao banco
    static void HealthCheckDatabase()
    {
        using var db = new ApplicationContext();
        //3atual
        //faz um select 1, para saber se existe conecção com o banco.
        var canConnect = db.Database.CanConnect();
        if (canConnect)
            Console.WriteLine("Posso me conectar!");

        #region dessas formas ou a forma acima
        try
        {
            //1 legado
            var connection = db.Database.GetDbConnection();
            connection.Open();

            //2 legado
            db.Departments.Any();
            Console.WriteLine("Posso me conectar!");
        }
        catch (Exception)
        {
            Console.WriteLine("Não posso me conectar!");
        }
        #endregion
    }


    static int _count;
    static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
    {
        using var db = new ApplicationContext();

        var time = Stopwatch.StartNew();

        var instanciaConexao = db.Database.GetDbConnection();
        instanciaConexao.StateChange += (_, __) => ++_count;


        if (gerenciarEstadoConexao)
        {
            instanciaConexao.Open();
        }

        for (var i = 0; i < 200; i++)
        {
            db.Departments.AsNoTracking().Any();
        }
        time.Stop();

        var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";

        Console.WriteLine(mensagem);
    }

    static void ExecuteSQL()
    {
        using var db = new ApplicationContext();

        //primeira opção
        using (var cmd = db.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText = "SELECT 1";
            cmd.ExecuteNonQuery();
        }

        string description = "teste";
        //segunda opção, parametros segue a ideia de sqlParameter para evitar sqlinjection
        db.Database.ExecuteSqlRaw("UPDATE Departments SET description={0} WHERE id =1", description);

        //terceira opção
        db.Database.ExecuteSqlInterpolated($"UPDATE Departments SET description={description} WHERE id=1");
    }

    static void SqlInjection()
    {
        using var db = new ApplicationContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Departments.AddRange(
            new Department
            {
                Description = "Departamento 01"
            },
            new Department
            {
                Description = "Departamento 02"
            }); ;

        db.SaveChanges();


        //var description = "Departamento 01";
        //db.Database.ExecuteSqlRaw("update Departments set description='DepartamentoAlterado' where description={0}", description);

        //injetando
        var description = "Teste 'or 1='1";
        db.Database.ExecuteSqlRaw($"update Departments set description='AtaqueSqlInjection' where description='{description}'");

        foreach (var departament in db.Departments.AsNoTracking())
        {
            Console.WriteLine($"Id: {departament.Id}, Descrição: {departament.Description}");
        }
    }

    static void ObterMigracoesPendentes()
    {
        //migração que foi gerada mas ainda não foi aplicada ao banco.
        using var db = new ApplicationContext();
        var migracoesPendentes = db.Database.GetPendingMigrations();

        Console.WriteLine($"Total: {migracoesPendentes.Count()}");

        foreach (var migracao in migracoesPendentes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }

    static void AplicarMigracaoEmTempoDeExecucao()
    {
        //ao executar o projeto, consulta as migracoes pendentes e aplica todas as migrações pendentes(update database em tempo de execução).
        var db = new ApplicationContext();
        db.Database.Migrate();
    }

    static void ObterTodasMigracoes()
    {
        //traz todas as migrações existentes na sua aplicação em tempo de execução.
        using var db = new ApplicationContext();
        var migracoesPendentes = db.Database.GetMigrations();

        Console.WriteLine($"Total: {migracoesPendentes.Count()}");

        foreach (var migracao in migracoesPendentes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }

    static void ObterMigracoesJaAplicadas()
    {
        //traz todas as migrações existentes na sua aplicação em tempo de execução.
        using var db = new ApplicationContext();
        var migracoesPendentes = db.Database.GetAppliedMigrations();

        Console.WriteLine($"Total: {migracoesPendentes.Count()}");

        foreach (var migracao in migracoesPendentes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }

    static void ScriptGeralDoBancoDeDados()
    {
        using var db = new ApplicationContext();
        var script = db.Database.GenerateCreateScript();

        Console.WriteLine(script);
    }

    #region TIPOS DE CARREGAMENTOS
    static void SetupTiposCarregamentos(ApplicationContext db)
    {
        if (!db.Departments.Any())
        {
            db.Departments.AddRange(
            new Department
            {
                Description = "Departamento 01",
                EmployeeList = new List<Employee>
                {
                    new Employee
                    {
                        Name = "Junior Silveira",
                        CPF = "85545569989",
                        RG = "2100062"
                    }
                }
            },
            new Department
            {
                Description = "Departamento 02",
                EmployeeList = new List<Employee>
                {
                    new Employee
                    {
                        Name = "Bruno Mesquita",
                        CPF = "555555533333",
                        RG = "8997778"
                    },
                    new Employee
                    {
                        Name = "João Gomes",
                        CPF = "77777777777",
                        RG = "445454544"
                    }
                }
            });
        }

        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    static void CarregamentoAdiantado()
    {
        using var db = new ApplicationContext();
        SetupTiposCarregamentos(db);

        var departmentList = db
            .Departments
            .Include(e => e.EmployeeList);

        foreach (var department in departmentList)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Departamento: {department.Description}");

            if (department.EmployeeList?.Any() ?? false)
            {
                foreach (var employee in department.EmployeeList)
                {
                    Console.WriteLine($"\tFuncionarios: {employee.Name}");
                }
            }
            else
            {
                Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }

    }

    static void CarregamentoExplicito()
    {
        using var db = new ApplicationContext();
        SetupTiposCarregamentos(db);

        var departmentList = db
            .Departments
            .ToList();

        foreach (var department in departmentList)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Departamento: {department.Description}");

            if (department.Id == 2)
            {
                //db.Entry(department).Collection(e => e.EmployeeList).Load();
                db.Entry(department).Collection(e => e.EmployeeList).Query().Where(e => e.Id > 2).ToList();
            }

            if (department.EmployeeList?.Any() ?? false)
            {
                foreach (var employee in department.EmployeeList)
                {
                    Console.WriteLine($"\tFuncionarios: {employee.Name}");
                }
            }
            else
            {
                Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }

    }

    static void CarregamentoLento()
    {
        /// <summary>
        /// Lazy loading carrega as entidades de navegação por demanda, quando a mesma for acessada ou consultada.
        /// no exemplo é => department.EmployeeList?.Any() ?? false
        /// nesse momento ele carrega a prop.
        /// o problema disso é que nesse ex de lista se tivessemos 10mil departamentos ele faria a primeira consulta so da entidade
        /// e depois 10mil consulta para cada interação do novo departamento. 10mil conexoes, 10mil interações.
        /// </summary>

        using var db = new ApplicationContext();
        SetupTiposCarregamentos(db);

        //desabilita lazy loading
        //db.ChangeTracker.LazyLoadingEnabled= false;

        var departmentList = db
            .Departments
            .ToList();

        foreach (var department in departmentList)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Departamento: {department.Description}");

            if (department.EmployeeList?.Any() ?? false)
            {
                foreach (var employee in department.EmployeeList)
                {
                    Console.WriteLine($"\tFuncionarios: {employee.Name}");
                }
            }
            else
            {
                Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }
    }
    #endregion

    #endregion

    #region MODULO CONSULTAS

  


    #endregion
}