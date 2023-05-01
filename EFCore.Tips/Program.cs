

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
        //ConsultaFiltrada();
        //SingleOrDefaultVsFirstOrDefault();
        //SemChavePrimaria();
        //ToView();
        //NaoUnicode();
        OperadoresDeAgregacao();
    }
    static void OperadoresDeAgregacao()
    {
        using var db = new ApplicationContext();

        var sql = db.Departamentos
            .GroupBy(p => p.Descricao)
            .Select(p =>
                new
                {
                    Descricao = p.Key,
                    Contador = p.Count(),
                    Media = p.Average(p => p.Id),
                    Maximo = p.Max(p => p.Id),
                    Soma = p.Sum(p => p.Id)
                }).ToQueryString();

        Console.WriteLine(sql);
    }
    static void NaoUnicode()
    {
        using var db = new ApplicationContext();
        var sql = db.Database.GenerateCreateScript();
        Console.WriteLine(sql);
    }

    static void ToView()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        //criando view e sua consulta
        db.Database.ExecuteSqlRaw(
            @"CREATE VIEW vw_departamento_relatorio AS
                SELECT
                    d.Descricao, count(c.Id) as Colaboradores
                FROM Departamentos d 
                LEFT JOIN Colaboradores c ON c.DepartamentoId=d.Id
                GROUP BY d.Descricao");

        //adicionando departamentos e colaboradores.
        var departamentos = Enumerable.Range(1, 10)
            .Select(p => new Departamento
            {
                Descricao = $"Departamento {p}",
                Colaboradores = Enumerable.Range(1, p)
                    .Select(c => new Colaborador
                    {
                        Nome = $"Colaborador {p}-{c}"
                    }).ToList()
            });

        //adicionando departameto sem colaborador
        var departamento = new Departamento
        {
            Descricao = $"Departamento Sem Colaborador"
        };

        //salvando
        db.Departamentos.Add(departamento);
        db.Departamentos.AddRange(departamentos);
        db.SaveChanges();


        //consultando view relatório.
        var relatorio = db.DepartamentoRelatorio
            .Where(p => p.Colaboradores < 4)
            .OrderBy(p => p.Departamento)
            .ToList();

        foreach (var dep in relatorio)
        {
            Console.WriteLine($"{dep.Departamento} [ Colaboradores: {dep.Colaboradores}]");
        }
    }
    static void SemChavePrimaria()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated(); //efcore cria e gerencia migrações das tabelas sem pk, porém o recurso está limitado a somente consulta.

        var usuarioFuncoes = db.UsuarioFuncoes.Where(p => p.UsuarioId == Guid.NewGuid()).ToArray();
    }
    static void SingleOrDefaultVsFirstOrDefault()
    {
        using var db = new ApplicationContext();

        Console.WriteLine("SingleOrDefault:");

        _ = db.Departamentos.SingleOrDefault(p => p.Id > 2);

        Console.WriteLine("FirstOrDefault:");

        _ = db.Departamentos.FirstOrDefault(p => p.Id > 2);
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