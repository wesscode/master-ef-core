// See https://aka.ms/new-console-template for more information
using MasterEFCore.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

Console.WriteLine("Hello, World!");
//EnsureCreatedAndDelete();
GapDoEnsureCreated();

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
