

using EFCore.Tips.Data;
using EFCore.Tips.Domain;
using Microsoft.EntityFrameworkCore;

static class Program
{
    static void Main(string[] args)
    {
        //ToQueryString();
        DebugView();
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