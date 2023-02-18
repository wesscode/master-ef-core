// See https://aka.ms/new-console-template for more information
using MasterEFCore.Data;

Console.WriteLine("Hello, World!");
//EnsureCreatedAndDelete();

static void EnsureCreatedAndDelete()
{
    //cria e dropa o banco
    using var db = new ApplicationContext();
    db.Database.EnsureCreated();
    //db.Database.EnsureDeleted();
}
