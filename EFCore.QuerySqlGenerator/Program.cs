using EFCore.QuerySqlGenerator;
using EFCore.QuerySqlGenerator.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

static class Program
{
    static void Main(string[] args)
    {
        DiagnosticListener.AllListeners.Subscribe(new MyInterceptorListener()); //increvendo meu listener

        using var db = new ApplicationContext();
        db.Database.EnsureCreated();

        _ = db.Departamentos.Where(x => x.Id > 0).ToArray();

        //var sql = db.Departamentos.Where(p => p.Id > 0).ToQueryString();
        //Console.WriteLine(sql);
    }
}