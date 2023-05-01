using EFCore.Testes.Data;
using EFCore.Testes.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Testes
{
    public class SQLiteTest
    {
        [Theory]
        [InlineData("Tecnologia")]
        [InlineData("Financeiro")]
        [InlineData("Departamento Pessoal")]
        public void Deve_inserir_e_consultar_um_departamento(string descricao)
        {
            //Arrange
            var departamento = new Departamento
            {
                Descricao = descricao,
                DataCadastro = DateTime.Now
            };

            //Setup
            var context = CreateContext();
            context.Database.EnsureCreated();
            context.Departamentos.Add(departamento);

            //Act
            var inseridos = context.SaveChanges();
            departamento = context.Departamentos.FirstOrDefault(p => p.Descricao == descricao);

            //Assert
            Assert.Equal(1, inseridos);
            Assert.Equal(descricao, departamento.Descricao);
        }

        private ApplicationContext CreateContext()
        {
            //conexao estanciada fora, pois aqui nos temos o controle da conection,
            //de quando deve abrir e fechar, quando está diretamente no optionsBuilder,
            //o entity trata isso em automatico(gerencia a conexao), logo não dando para rodar um banco relacional
            //em memoria pq ele vai rodar como um transient.
            var conexao = new SqliteConnection("Datasource=:memory:");
            conexao.Open();

            var optionsBuilder  = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(conexao)
                .Options;

            return new ApplicationContext(optionsBuilder);
        }
    }
}