using EFCore.Testes.Data;
using EFCore.Testes.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Testes
{
    public class InMemoryTest
    {
        [Fact]
        public void Deve_inserir_um_departamento()
        {
            //Arrange
            var departamento = new Departamento
            {
                Descricao = "Tecnologia",
                DataCadastro = DateTime.Now
            };

            //Setup
            var context = CreateContext();
            context.Departamentos.Add(departamento);

            //Act
            var inseridos = context.SaveChanges();

            //Assert
            Assert.Equal(1, inseridos);
        }

        [Fact]
        public void Nao_implementado_funcoes_de_datas_para_o_provider_inmemory()
        {
            //Arrange
            var departamento = new Departamento
            {
                Descricao = "Tecnologia",
                DataCadastro = DateTime.Now
            };

            //Setup
            var context = CreateContext();
            context.Departamentos.Add(departamento);

            //Act
            var inseridos = context.SaveChanges();

            //Assert
            Action action = () => context
                .Departamentos
                .FirstOrDefault(p => EF.Functions.DateDiffDay(DateTime.Now, p.DataCadastro) > 0); //verifica se a diferenças das datas é maior que 0;
            //ef functions não funciona com o provider inmemory, pois são ações executadas no banco, localmente não possui suporte.
            Assert.Throws<InvalidOperationException>(action);
        }

        private ApplicationContext CreateContext()
        {
            var optionsBuilder  = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("InMemoryTest")
                .Options;
            return new ApplicationContext(optionsBuilder);
        }
    }
}