

using EFCore.Tips.Data;
using Microsoft.EntityFrameworkCore;

static class Program
{
    static void Main(string[] args)
    {
        ToQueryString();
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