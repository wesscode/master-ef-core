

using EFCore.Tips.Data;
using EFCore.Tips.Domain;
using Microsoft.EntityFrameworkCore;

static class Program
{
    static void Main(string[] args)
    {
        //ToQueryString();
        //DebugView();
        //Clear();
        ConsultaFiltrada();
    }
    static void ConsultaFiltrada()
    {
        using var db = new ApplicationContext();

        var sql = db
            .Departamentos
            .Include(p => p.Colaboradores.Where(c => c.Nome.Contains("Teste")))
            .ToQueryString();

        Console.WriteLine(sql);
    }

    static void Clear()
    {
        using var db = new ApplicationContext();

        db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });

        db.ChangeTracker.Clear(); //Redefine o estado do contexto.
    }
    static void DebugView()
    {
        using var db = new ApplicationContext();

        db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });

        var query = db.Departamentos.Where(p => p.Id > 2);
    }

    static void ToQueryString()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureCreated();

        var query = db.Departamentos.Where(x => x.Id > 2);
        var sql = query.ToQueryString();
        Console.WriteLine(sql);
    }
}