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
        //EnsureCreatedAndDelete();
        //GapDoEnsureCreated();
        //HealthCheckDatabase();

        //new ApplicationContext().departments.AsNoTracking().Any();
        //_count = 0;
        //GerenciarEstadoDaConexao(false);
        //_count = 0;
        //GerenciarEstadoDaConexao(true);

        SqlInjection();
    }


    static void EnsureCreatedAndDelete()
    {
        //cria e dropa o banco
        using var db = new ApplicationContext();
        db.Database.EnsureCreated();
        //db.Database.EnsureDeleted();
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

    static void HealthCheckDatabase()
    {
        using var db = new ApplicationContext();
        //3atual
        //faz um select 1, para saber se existe conecção com o banco.
        var canConnect = db.Database.CanConnect();
        if (canConnect)
            Console.WriteLine("Posso me conectar!");


        try
        {
            //1 legado
            var connection = db.Database.GetDbConnection();
            connection.Open();

            //2 legado
            db.departments.Any();
            Console.WriteLine("Posso me conectar!");
        }
        catch (Exception)
        {

            Console.WriteLine("Não posso me conectar!");
        }
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
            db.departments.AsNoTracking().Any();
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

        db.departments.AddRange(
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

        foreach (var departament in db.departments.AsNoTracking())
        {
            Console.WriteLine($"Id: {departament.Id}, Descrição: {departament.Description}");
        }
    }

}

