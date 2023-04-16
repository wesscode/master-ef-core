// See https://aka.ms/new-console-template for more information
using MasterEFCore.Data;
using MasterEFCore.Domain;
using MasterEFCore.Funcoes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Transactions;

static class Program
{
    static void Main(string[] args)
    {
        #region MODULO INICIAL ATÉ TIPO DE CARREGAMENTO
        //EnsureCreatedAndDelete();

        //GapDoEnsureCreated();

        //HealthCheckDatabase();

        //new ApplicationContext().Departments.AsNoTracking().Any();
        //_count = 0;
        //GerenciarEstadoDaConexao(false);
        //_count = 0;
        //GerenciarEstadoDaConexao(true);

        //SqlInjection();

        //ObterMigracoesPendentes();

        //AplicarMigracaoEmTempoDeExecucao();

        //ObterTodasMigracoes();

        //ObterMigracoesJaAplicadas();

        //ScriptGeralDoBancoDeDados();

        //CarregamentoAdiantado();

        //CarregamentoExplicito();

        //CarregamentoLento();

        #endregion

        #region MODULO CONSULTA E PROCEDURES
        //FiltroGlobal();

        // IgnoreFiltroGlobal();

        //ConsultaProjetada();

        //ConsultaParametrizada();

        //ConsultaInterpolada();

        //ConsultaComTag();

        //EntendendoConsulta1NN1();

        //DivisaoDeConsulta();

        //CriarStoreProcedure();

        //InserirDadosViaProcedure();

        //CriarStoreProcedureDeConsulta();

        //ConsultaViaProcedure();
        #endregion

        #region MODULO INFRAESTRUTURA
        // ConsultarDepartamentos();
        //DadosSensiveis();
        //HabilitandoBatchSize();
        //TempoComandoGeral();
        #endregion

        #region MODULO MODELO DE DADOS
        //Collations();
        //TesteCollations();
        //PropagarDados();
        //Esquema();
        //ConversorDeValor();
        //ConversorCustomizado();
        //PropriedadeDeSombra();
        //TrabalhandoComPropriedadeDeSombra();
        //TiposDePropriedades();
        //Relacionamento1Para1();
        //Relacionamento1ParaMuitos();
        //RelacionamentoMuitosParaMuitos();
        //CampoDeApoio();
        //ExemploTPH();
        //PacotesDePropriedades();
        #endregion

        #region MODULO DATAANNOTATIONS
        //Atributos();
        #endregion

        #region MODULO EF FUNCTION
        //FuncoesDeDatas();
        //FuncaoLike();
        //FuncaoDataLength();
        //FuncaoProperty();
        //FucaoCollate();
        #endregion

        #region MODULO INTERCEPTAÇÃO
        //TesteInterceptacao();
        //TesteInterceptacaoSaveChanges();
        #endregion

        #region MODULO TRANSAÇÕES
        //ComportamentoPadrao();
        //GerenciandoTransacaoManualmente();
        //ReverterTransacao();
        //SalvarPontoTransacao();
        //TransactionScope();
        #endregion

        #region MODULO UDFs
        //FuncaoLEFT();
        // FuncaoDefinidaPeloUsuario();
        #endregion

        #region MODULO PEFORMANCE
        //Setup();
        //ConsultaRastreada();
        //ConsultaNaoRastreada();
        //ConsultaComResolucaoDeIdentidade();
        ConsultaCustomizada();
        #endregion

    }

    #region MODULO INICIAL ATÉ TIPO DE CARREGAMENTO
    /*
     static void EnsureCreatedAndDelete()
    {
        //cria e dropa o banco em tempo de execução.
        using var db = new ApplicationContext();
        //db.Database.EnsureCreated();
        db.Database.EnsureDeleted();
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
    //checando se estar conectando ao banco
    static void HealthCheckDatabase()
    {
        using var db = new ApplicationContext();
        //3atual
        //faz um select 1, para saber se existe conecção com o banco.
        var canConnect = db.Database.CanConnect();
        if (canConnect)
            Console.WriteLine("Posso me conectar!");

        #region dessas formas ou a forma acima
        try
        {
            //1 legado
            var connection = db.Database.GetDbConnection();
            connection.Open();

            //2 legado
            db.Departments.Any();
            Console.WriteLine("Posso me conectar!");
        }
        catch (Exception)
        {
            Console.WriteLine("Não posso me conectar!");
        }
        #endregion
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
            db.Departments.AsNoTracking().Any();
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

        db.Departments.AddRange(
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

        foreach (var departament in db.Departments.AsNoTracking())
        {
            Console.WriteLine($"Id: {departament.Id}, Descrição: {departament.Description}");
        }
    }
    static void ObterMigracoesPendentes()
    {
        //migração que foi gerada mas ainda não foi aplicada ao banco.
        using var db = new ApplicationContext();
        var migracoesPendentes = db.Database.GetPendingMigrations();

        Console.WriteLine($"Total: {migracoesPendentes.Count()}");

        foreach (var migracao in migracoesPendentes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }
    static void AplicarMigracaoEmTempoDeExecucao()
    {
        //ao executar o projeto, consulta as migracoes pendentes e aplica todas as migrações pendentes(update database em tempo de execução).
        var db = new ApplicationContext();
        db.Database.Migrate();
    }
    static void ObterTodasMigracoes()
    {
        //traz todas as migrações existentes na sua aplicação em tempo de execução.
        using var db = new ApplicationContext();
        var migracoesPendentes = db.Database.GetMigrations();

        Console.WriteLine($"Total: {migracoesPendentes.Count()}");

        foreach (var migracao in migracoesPendentes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }
    static void ObterMigracoesJaAplicadas()
    {
        //traz todas as migrações existentes na sua aplicação em tempo de execução.
        using var db = new ApplicationContext();
        var migracoesPendentes = db.Database.GetAppliedMigrations();

        Console.WriteLine($"Total: {migracoesPendentes.Count()}");

        foreach (var migracao in migracoesPendentes)
        {
            Console.WriteLine($"Migração: {migracao}");
        }
    }
    static void ScriptGeralDoBancoDeDados()
    {
        using var db = new ApplicationContext();
        var script = db.Database.GenerateCreateScript();

        Console.WriteLine(script);
    }
     */

    #region TIPOS DE CARREGAMENTOS
    /*
     static void SetupTiposCarregamentos(ApplicationContext db)
    {
        if (!db.Departments.Any())
        {
            db.Departments.AddRange(
            new Department
            {
                Description = "Departamento 01",
                EmployeeList = new List<Employee>
                {
                    new Employee
                    {
                        Name = "Junior Silveira",
                        CPF = "85545569989",
                        RG = "2100062"
                    }
                }
            },
            new Department
            {
                Description = "Departamento 02",
                EmployeeList = new List<Employee>
                {
                    new Employee
                    {
                        Name = "Bruno Mesquita",
                        CPF = "555555533333",
                        RG = "8997778"
                    },
                    new Employee
                    {
                        Name = "João Gomes",
                        CPF = "77777777777",
                        RG = "445454544"
                    }
                }
            });
        }

        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    static void CarregamentoAdiantado()
    {
        using var db = new ApplicationContext();
        SetupTiposCarregamentos(db);

        var departmentList = db
            .Departments
            .Include(e => e.EmployeeList);

        foreach (var department in departmentList)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Departamento: {department.Description}");

            if (department.EmployeeList?.Any() ?? false)
            {
                foreach (var employee in department.EmployeeList)
                {
                    Console.WriteLine($"\tFuncionarios: {employee.Name}");
                }
            }
            else
            {
                Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }

    }

    static void CarregamentoExplicito()
    {
        using var db = new ApplicationContext();
        SetupTiposCarregamentos(db);

        var departmentList = db
            .Departments
            .ToList();

        foreach (var department in departmentList)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Departamento: {department.Description}");

            if (department.Id == 2)
            {
                //db.Entry(department).Collection(e => e.EmployeeList).Load();
                db.Entry(department).Collection(e => e.EmployeeList).Query().Where(e => e.Id > 2).ToList();
            }

            if (department.EmployeeList?.Any() ?? false)
            {
                foreach (var employee in department.EmployeeList)
                {
                    Console.WriteLine($"\tFuncionarios: {employee.Name}");
                }
            }
            else
            {
                Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }

    }

    static void CarregamentoLento()
    {
        /// <summary>
        /// Lazy loading carrega as entidades de navegação por demanda, quando a mesma for acessada ou consultada.
        /// no exemplo é => department.EmployeeList?.Any() ?? false
        /// nesse momento ele carrega a prop.
        /// o problema disso é que nesse ex de lista se tivessemos 10mil departamentos ele faria a primeira consulta so da entidade
        /// e depois 10mil consulta para cada interação do novo departamento. 10mil conexoes, 10mil interações.
        /// </summary>

        using var db = new ApplicationContext();
        SetupTiposCarregamentos(db);

        //desabilita lazy loading
        //db.ChangeTracker.LazyLoadingEnabled= false;

        var departmentList = db
            .Departments
            .ToList();

        foreach (var department in departmentList)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Departamento: {department.Description}");

            if (department.EmployeeList?.Any() ?? false)
            {
                foreach (var employee in department.EmployeeList)
                {
                    Console.WriteLine($"\tFuncionarios: {employee.Name}");
                }
            }
            else
            {
                Console.WriteLine($"\tNenhum funcionario encontrado!");
            }
        }
    }
     */
    #endregion

    #endregion

    #region MODULO CONSULTAS E PROCEDURES
    /*
       static void ConsultaViaProcedure()
      {

          using var db = new ApplicationContext();

          var dep = new SqlParameter("@dep", "Departamento");
          var departamentos = db.Departments
              //.FromSqlRaw("EXECUTE GetDepartments @dep", dep)
              .FromSqlInterpolated($"EXECUTE GetDepartments {dep}")
              .ToList();
          foreach (var departamento in departamentos)
          {
              Console.WriteLine($"Descricao: {departamento.Description}");
          }
      }
      static void CriarStoreProcedureDeConsulta()
      {
          var criaConsultaDepartamentos = @"
              CREATE OR ALTER PROCEDURE GetDepartments
                  @Description VARCHAR(50)
              AS 
              BEGIN
                SELECT * FROM Departments WHERE Description LIKE @Description + '%'
              END
              ";

          using var db = new ApplicationContext();
          db.Database.ExecuteSqlRaw(criaConsultaDepartamentos);
      }
      static void InserirDadosViaProcedure()
      {
          using var db = new ApplicationContext();
          db.Database.ExecuteSqlRaw("EXECUTE CreateDepartment @p0, @p1", "Departamento via Procedure II", true);
      }
      static void CriarStoreProcedure()
      {
          var criarDepartamento = @"
              CREATE OR ALTER PROCEDURE CreateDepartment
                  @Description VARCHAR(50),
                  @Active bit
              AS 
              BEGIN
                  INSERT INTO 
                      Departments(Description, Active, IsDeleted) 
                  VALUES (@Description, @Active, 0);
              END
              ";

          using var db = new ApplicationContext();
          db.Database.ExecuteSqlRaw(criarDepartamento);
      }
      static void DivisaoDeConsulta()
      {
          using var db = new ApplicationContext();
          Setup(db);

          var departaments = db.Departments
              .Include(e => e.EmployeeList)
              .Where(p => p.Id < 3)
              //.AsSplitQuery() //splitQuery Local
              //.AsSingleQuery() //desativa splitQueyGlobal
              .ToList();

          foreach (var department in departaments)
          {
              Console.WriteLine($"Descrição: {department.Description}");
              foreach (var employee in department.EmployeeList)
              {
                  Console.WriteLine($"\tNome: {employee.Name}");
              }
          }
      }
      static void EntendendoConsulta1NN1()
      {
          using var db = new ApplicationContext();
          Setup(db);

          var funcionarioList = db
             .Employees
             .Include(e => e.Department)
             .ToList();

          foreach (var employee in funcionarioList)
          {
              Console.WriteLine($"Descrição: {employee.Name} / Descrição Dep: {employee.Department.Description}");
          }


          //var departamentList = db
          //    .Departments
          //    .Include(e => e.EmployeeList)
          //    .ToList();

          //foreach (var department in departamentList)
          //{
          //    Console.WriteLine($"Descrição: {department.Description}");

          //    foreach (var employee in department.EmployeeList)
          //    {
          //        Console.WriteLine($"\tName: {employee.Name}");
          //    }
          //}
      }
      static void ConsultaComTag()
      {
          using var db = new ApplicationContext();
          Setup(db);

          var departamentList = db
              .Departments
              .TagWith(@"Estou Enviando um comentário para o servidor!
                         Segundo comentário!
                         Terceiro comentário!")
              .ToList();

          foreach (var department in departamentList)
          {
              Console.WriteLine($"Descrição: {department.Description}");
          }
      }
      static void ConsultaInterpolada()
      {
          using var db = new ApplicationContext();
          Setup(db);

          var id = 1;
          var departamentList = db
              .Departments
              .FromSqlInterpolated($"SELECT * FROM Departments WHERE id >{id}")
              .ToList();

          foreach (var department in departamentList)
          {
              Console.WriteLine($"Descrição: {department.Description}");
          }
      }
      static void ConsultaParametrizada()
      {
          using var db = new ApplicationContext();
          Setup(db);

          //passar parametros assim
          //int id = 0;

          //OU
          var id = new SqlParameter
          {
              Value = 1,
              SqlDbType = System.Data.SqlDbType.Int
          };

          var departamentList = db
              .Departments
              //.FromSqlRaw("SELECT * FROM Departments WITH(NOLOCK)") //WITH(NOLOCK) traz leitura suja misturados com dados confirmados, ajuda na peformance.
              .FromSqlRaw("SELECT * FROM Departments WHERE id >{0}", id)
              .Where(x => !x.IsDeleted) //consulta raw, fazendo composição com o Linq
              .ToList();

          foreach (var department in departamentList)
          {
              Console.WriteLine($"Descrição: {department.Description}");

          }
      }
      static void ConsultaProjetada()
      {
          using var db = new ApplicationContext();
          Setup(db);

          var departamentList = db
              .Departments
              .Where(x => x.Id > 0)
              .Select(p => new
              {
                  p.Description,
                  Funcionarios = p.EmployeeList.Select(n => n.Name)
              })
              .ToList();

          foreach (var department in departamentList)
          {
              Console.WriteLine($"Descrição: {department.Description}");
              foreach (var funcionario in department.Funcionarios)
              {
                  Console.WriteLine($"Nome: {funcionario}");
              }
          }
      }
      static void IgnoreFiltroGlobal()
      {
          using var db = new ApplicationContext();
          Setup(db);

          var departamentList = db.Departments.IgnoreQueryFilters().Where(x => x.Id > 0).ToList();

          foreach (var department in departamentList)
          {
              Console.WriteLine($"Descrição: {department.Description} \t Excluido: {department.IsDeleted}");
          }
      }
      static void FiltroGlobal()
      {
          using var db = new ApplicationContext();
          Setup(db);

          var departamentList = db.Departments.Where(x => x.Id > 0).ToList();

          foreach (var department in departamentList)
          {
              Console.WriteLine($"Descrição: {department.Description} \t Excluido: {department.IsDeleted}");
          }
      }
      static void Setup(ApplicationContext db)
      {
          if (db.Database.EnsureCreated())
          {
              db.Departments.AddRange(
              new Department
              {
                  Description = "Departamento 01",
                  Active = true,
                  IsDeleted = true,

                  EmployeeList = new List<Employee>
                  {
                      new Employee
                      {
                          Name = "Junior Silveira",
                          CPF = "85545569989",
                          RG = "2100062"
                      }
                  }
              },
              new Department
              {
                  Description = "Departamento 02",
                  Active = true,
                  EmployeeList = new List<Employee>
                  {
                      new Employee
                      {
                          Name = "Bruno Mesquita",
                          CPF = "555555533333",
                          RG = "8997778"
                      },
                      new Employee
                      {
                          Name = "João Gomes",
                          CPF = "77777777777",
                          RG = "445454544"
                      }
                  }
              });
              db.SaveChanges();
              db.ChangeTracker.Clear();
          }
      }
     */
    #endregion

    #region MODULO INFRAESTRUTURA
    /*
      static void ExecutarEstrategiaResiliencia()
     {
         using var db = new ApplicationContext();

         var strategy = db.Database.CreateExecutionStrategy();
         strategy.Execute(() =>
         {
             using var transaction = db.Database.BeginTransaction();

             db.Departments.Add(new Department { Description = "Departamento Transacao" });
             db.SaveChanges();

             transaction.Commit();
         });
     }

     static void TempoComandoGeral()
     {
         //TEMPO LIMITE DE UM COMANDO.
         using var db = new ApplicationContext();

         db.Database.SetCommandTimeout(10);
         db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1");
     }

     static void HabilitandoBatchSize()
     {
         using var db = new ApplicationContext();
         db.Database.EnsureDeleted();
         db.Database.EnsureCreated();

         for (var i = 0; i < 50; i++)
         {
             db.Departments.Add(new Department
             {
                 Description = "Departamento " + i
             });
         }
         db.SaveChanges();
     }

     static void DadosSensiveis()
     {
         using var db = new ApplicationContext();
         var descricao = "Departamento";
         var departamentos = db.Departments.Where(x => x.Description == descricao).ToArray();
     }

     static void ConsultarDepartamentos()
     {
         using var db = new ApplicationContext();

         var departamentos = db.Departments.Where(x => x.Id > 0).ToArray();
     }
     */
    #endregion

    #region MODULO MODELO DE DADOS
    /*
    static void Collations()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Departments.AddRange(
            new Department
            {
                Description = "Departamento 01",
                Active = false,
                IsDeleted = false
            },
        new Department
        {
            Description = "departamento 01",
            Active = true,
            IsDeleted = false,
            EmployeeList = new List<Employee>
            {
                new Employee
                {
                    Name="employee",
                    RG="123",
                    CPF="321",
                },
                new Employee
                {
                    Name="Employee",
                    RG="456",
                    CPF="654",
                }
            }
        });

        db.SaveChanges();
    }
    static void TesteCollations()
    {
        using var db = new ApplicationContext();

        var departmentList = db.Departments.Where(x => x.Description == "Departamento 01").ToList();
        var employeeList = db.Employees.Where(x => x.Name == "employee").ToList();

        foreach (var item1 in departmentList)
        {

            Console.WriteLine("department: " + item1.Description);
        }

        foreach (var item2 in employeeList)
        {
            Console.WriteLine("EMPLOYEE " + item2.Name);

        }

    }

    static void PropagarDados()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var script = db.Database.GenerateCreateScript();
        Console.WriteLine(script);
    }

    static void Esquema()
    {
        using var db = new ApplicationContext();

        var script = db.Database.GenerateCreateScript();
        Console.WriteLine(script);
    }

    static void ConversorDeValor() => Esquema();

    static void ConversorCustomizado()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Conversores.Add(
            new Conversor
            {
                Status = Status.Devolvido
            });

        db.SaveChanges();

        var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Analise);

        var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Devolvido);
    }
   

    static void PropriedadeDeSombra()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
     */
    static void TrabalhandoComPropriedadeDeSombra()
    {
        using var db = new ApplicationContext();

        /*
         * INSERT SHADOW PROPERTY
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var departamento = new Department
        {
            Description = "Departamento propriedade de sombra."
        };
        db.Departments.Add(departamento);
        //Shadow prop
        db.Entry(departamento).Property("LastUpdate").CurrentValue = DateTime.Now;

        db.SaveChanges();
        */

        //GET SHADOW PROPERTY
        //var departamentos = db.Departments.Where(p => EF.Property<DateTime>(p, "LastUpdate") < DateTime.Now).ToArray();
    }
    /*
    static void TiposDePropriedades() //Owner Types
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var client = new Client
        {
            Name = "Fulano de tal",
            Tel = "(85) 98945-9898",
            Adress = new Adress { District = "Centro", City = "Fortaleza" }
        };

        db.Clients.Add(client);
        db.SaveChanges();
    }

    static void Relacionamento1Para1()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        Estate estate = new Estate
        {
            Name = "Sergipe",
            Governador = new Governador { Name = "Wesley Alves" }
        };

        db.Estates.Add(estate);
        db.SaveChanges();

        var estados = db.Estates.AsNoTracking().ToList();

        estados.ForEach(e =>
        {
            Console.WriteLine($"Estado: {e.Name}, Governador: {e.Governador.Name}");
        });
    }

    static void Relacionamento1ParaMuitos()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estate
            {
                Name = "Sergipe",
                Governador = new Governador { Name = "Wesley Alves" }
            };

            estado.Cities.Add(new City { Name = "Itabaiana" });

            db.Estates.Add(estado);
            db.SaveChanges();
        }

        using (var db = new ApplicationContext())
        {
            var estados = db.Estates.ToList();

            estados[0].Cities.Add(new City { Name = "Aracaju" });

            db.SaveChanges();

            foreach (var estado in db.Estates.Include(c => c.Cities).AsNoTracking())
            {
                Console.WriteLine($"Estado: {estado.Name}, Governador: {estado.Governador.Name}");

                foreach (var cidade in estado.Cities)
                {
                    Console.WriteLine($"\t Cidade: {cidade.Name}");
                }
            }
        }
    }

    static void RelacionamentoMuitosParaMuitos()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var ator1 = new Actor { Name = "Rafael" };
            var ator2 = new Actor { Name = "Pires" };
            var ator3 = new Actor { Name = "Bruno" };

            var filme1 = new Movie { Description = "A volta dos que não foram." };
            var filme2 = new Movie { Description = "De volta para o futuro." };
            var filme3 = new Movie { Description = "Poeira em alto mar filme." };

            ator1.MovieList.Add(filme1);
            ator1.MovieList.Add(filme2);

            ator2.MovieList.Add(filme1);

            filme3.ActorList.Add(ator1);
            filme3.ActorList.Add(ator2);
            filme3.ActorList.Add(ator3);

            db.AddRange(ator1, ator2, filme3);
            db.SaveChanges();

            foreach (var ator in db.Actors.Include(e => e.MovieList))
            {
                Console.WriteLine($"Ator: {ator.Name}");

                foreach (var movie in ator.MovieList)
                {
                    Console.WriteLine($"\tFilme: {movie.Description}");
                }
            }
        }
    }

    static void CampoDeApoio()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var documento = new Document();
            documento.SetCPF("1234566733");

            db.Documents.Add(documento);
            db.SaveChanges();

            foreach (var doc in db.Documents.AsNoTracking())
            {
                Console.WriteLine($"CPF -> {doc.GetCPF()}");
            }
        }
    }


    static void ExemploTPH()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var pessoa = new Person { Name = "Fulano de Tal" };

            var instrutor = new Instructor { Name = "Rafael Almeida", Tecnology = ".NET", Desde = DateTime.Now };

            var aluno = new Student { Name = "Maria Thysbe", Age = 31, ContractDate = DateTime.Now.AddDays(-1) };

            db.AddRange(pessoa, instrutor, aluno);
            db.SaveChanges();

            var pessoas = db.Persons.AsNoTracking().ToArray();
            var instrutores = db.Instructors.AsNoTracking().ToArray();
            //var alunos = db.Students.AsNoTracking().ToArray();
            var alunos = db.Persons.OfType<Student>().AsNoTracking().ToArray(); //Forma de consulta, sem precisar adicionar tabelas de herança ao DbContext

            Console.WriteLine("Pessoas ***************");
            foreach (var p in pessoas)
            {
                Console.WriteLine($"Id: {p.Id} -> {p.Name}");
            }

            Console.WriteLine("Instrutores ***************");
            foreach (var p in instrutores)
            {
                Console.WriteLine($"Id: {p.Id} -> {p.Name}, Tecnologia: {p.Tecnology}, Desde: {p.Desde}");
            }

            Console.WriteLine("Alunos ***************");
            foreach (var p in alunos)
            {
                Console.WriteLine($"Id: {p.Id} -> {p.Name}, Idade: {p.Age}, Data do Contrato: {p.ContractDate}");
            }
        }
    }

    static void PacotesDePropriedades()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var configuracao = new Dictionary<string, object>
            {
                ["Chave"] = "SenhaBancoDeDados",
                ["Valor"] = Guid.NewGuid().ToString()
            };

            db.Configuracoes.Add(configuracao);
            db.SaveChanges();

            var configuracoes = db
                .Configuracoes
                .AsNoTracking()
                .Where(x => x["Chave"] == "SenhaBancoDeDados")
                .ToArray();

            foreach (var dic in configuracoes)
            {
                Console.WriteLine($"Chave: {dic["Chave"]} - Valor: {dic["Valor"]}");
            }
        }
    }
    */
    #endregion

    #region Modulo MODULO DATAANNOTATIONS
    /*
    static void Atributos()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);

            db.Atributos.Add(new Atributo
            {
                Description = "Exemplo",
                Observation = "Observacao"
            });
            db.SaveChanges();
        }
    }
    */
    #endregion

    #region MODULO EF FUNCTION
    /*
    static void ApagarCriarBancoDeDados()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Funcoes.AddRange(
        new Funcao
        {
            Data1 = DateTime.Now.AddDays(2),
            Data2 = "2021-01-01",
            Description1 = "Bala 1",
            Description2 = "Bala 1"
        },
        new Funcao
        {
            Data1 = DateTime.Now.AddDays(1),
            Data2 = "XX21-01-01",
            Description1 = "Bola 2",
            Description2 = "Bola 2"
        },
        new Funcao
        {
            Data1 = DateTime.Now.AddDays(1),
            Data2 = "XX21-01-01",
            Description1 = "Tela",
            Description2 = "Tela"
        });

        db.SaveChanges();
    }
    static void FuncoesDeDatas()
    {
        ApagarCriarBancoDeDados();

        using (var db = new ApplicationContext())
        {
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);

            var dados = db.Funcoes.AsNoTracking().Select(p => new
            {
                Dias = EF.Functions.DateDiffDay(DateTime.Now, p.Data1),
                Meses = EF.Functions.DateDiffMonth(DateTime.Now, p.Data1),
                Data = EF.Functions.DateFromParts(2021, 1, 2),
                DataValida = EF.Functions.IsDate(p.Data2),
            });

            foreach (var f in dados)
            {
                Console.WriteLine(f);
            }

        }
    }
    static void FuncaoLike()
    {
        using (var db = new ApplicationContext())
        {
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);

            var dados = db.Funcoes
                .AsNoTracking()
                //.Where(p => EF.Functions.Like(p.Description1, "Bo%")) //like onde inicia com BO
                .Where(p => EF.Functions.Like(p.Description1, "B[ao]%")) //like onde inicia com Ba ou Bo 
                .Select(x => x.Description1)
                .ToArray();

            Console.WriteLine("Resultado: ");

            foreach (var descricao in dados)
            {
                Console.WriteLine(descricao);
            }
        }
    }
    static void FuncaoDataLength()
    {
        using (var db = new ApplicationContext())
        {
            var resultado = db.Funcoes
                .AsNoTracking()
                .Select(p => new
                {
                    TotalBytesCampoData = EF.Functions.DataLength(p.Data1),
                    TotalBytes1 = EF.Functions.DataLength(p.Description1),
                    TotalBytes2 = EF.Functions.DataLength(p.Description2),
                    Total1 = p.Description1.Length,
                    Total2 = p.Description2.Length
                })
                .FirstOrDefault();

            Console.WriteLine("Resultado: ");
            Console.WriteLine(resultado);
        }
    }
    static void FuncaoProperty()
    {
        ApagarCriarBancoDeDados();

        using (var db = new ApplicationContext())
        {
            var resultado = db
                .Funcoes
                .FirstOrDefault(p => EF.Property<string>(p, "PropriedadeSombra") == "Teste");

            var propriedadeSombra = db
                .Entry(resultado)
                .Property<string>("PropriedadeSombra")
                .CurrentValue;

            Console.WriteLine("Resultado: ");
            Console.WriteLine(propriedadeSombra);
        }
    }
    static void FucaoCollate()
    {
        using (var db = new ApplicationContext())
        {
            var consulta1 = db
                .Funcoes
                .FirstOrDefault(p => EF.Functions.Collate(p.Description1, "SQL_Latin1_General_CP1_CS_AS") == "tela");

            var consulta2 = db
                .Funcoes
                .FirstOrDefault(p => EF.Functions.Collate(p.Description1, "SQL_Latin1_General_CP1_CI_AS") == "tela");

            Console.WriteLine($"Consulta1: {consulta1?.Description1}");

            Console.WriteLine($"Consulta2: {consulta2?.Description1}");
        }
    }
    */
    #endregion

    #region MODULO INTERCEPTAÇÃO
    /*
    static void TesteInterceptacao()
    {
        using (var db = new ApplicationContext())
        {
            var consulta = db
                .Funcoes
                .TagWith("Use NOLOCK")
                .FirstOrDefault();

            Console.WriteLine($"Consultas: {consulta?.Description1}");
        }
    }
    static void TesteInterceptacaoSaveChanges()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Funcoes.Add(new Funcao
            {
                Description1 = "Teste"
            });

            db.SaveChanges();
        }
    }
    */
    #endregion

    #region MODULO TRANSAÇÕES
    /*
    static void TransactionScope()
    {
        CadastrarLivro();

        //definindo nv de isolamento.
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
        };

        using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
        {
            ConsultarAtualizar();
            CadastrarLivroEnterprise();
            CadastrarLivroEnterprise();

            scope.Complete();
        }
    }
    static void ConsultarAtualizar()
    {
        using (var db = new ApplicationContext())
        {
            var livro = db.Books.FirstOrDefault(p => p.Id == 1);
            livro.Autor = "Rafael Almeida";
            db.SaveChanges();
        }
    }
    static void CadastrarLivroEnterprise()
    {
        using (var db = new ApplicationContext())
        {
            db.Books.Add(
                new Book
                {
                    Titulo = "ASP.NET Core Enterprise Applications.",
                    Autor = "Eduardo Pires"
                });
            db.SaveChanges();
        }
    }
    static void CadastrarLivroDominandoEFCore()
    {
        using (var db = new ApplicationContext())
        {
            db.Books.Add(
                new Book
                {
                    Titulo = "Dominando Entity Framework Core",
                    Autor = "Rafael Almeida"
                });
            db.SaveChanges();
        }
    }
    static void SalvarPontoTransacao()
    {
        CadastrarLivro();

        using (var db = new ApplicationContext())
        {
            var transacao = db.Database.BeginTransaction();

            try
            {
                var livro = db.Books.FirstOrDefault(p => p.Id == 1);
                livro.Autor = "Rafael Almeida";
                db.SaveChanges();

                transacao.CreateSavepoint("desfazer_apenas_insercao"); //criando ponto de salvamento, para qnd dar rollback salvar somente o que foi alterado até aqui.

                db.Books.Add(
                   new Book
                   {
                       Titulo = "ASP.NET CORE Enterprise Applications",
                       Autor = "Eduardo Pires"
                   });
                db.SaveChanges();

                db.Books.Add(
                    new Book
                    {
                        Titulo = "Dominando o entity framework core",
                        Autor = "Rafael Almeida".PadLeft(16, '*')
                    });
                db.SaveChanges();

                transacao.Commit();
            }
            catch (DbUpdateException e)
            {
                transacao.RollbackToSavepoint("desfazer_apenas_insercao"); //rollback apartir do ponto definido.
                if (e.Entries.Count(p => p.State == EntityState.Added) == e.Entries.Count) //verificando count de entidades com o estado de added se é igual o count que estou enviando para a base, realizo o commit.
                {
                    transacao.Commit();
                }
            }
        }
    }
    static void ReverterTransacao()
    {
        CadastrarLivro();

        using (var db = new ApplicationContext())
        {
            var transacao = db.Database.BeginTransaction();

            try
            {
                var livro = db.Books.FirstOrDefault(p => p.Id == 1);
                livro.Autor = "Rafael Almeida";

                db.SaveChanges();

                db.Books.Add(
                    new Book
                    {
                        Titulo = "Dominando o entity framework core",
                        Autor = "Rafael Almeida".PadLeft(16, '*')
                    });

                db.SaveChanges();

                transacao.Commit();
            }
            catch (Exception e)
            {
                transacao.Rollback();
            }
        }
    }
    static void GerenciandoTransacaoManualmente()
    {
        CadastrarLivro();

        using (var db = new ApplicationContext())
        {
            var transacao = db.Database.BeginTransaction();


            var livro = db.Books.FirstOrDefault(p => p.Id == 1);
            livro.Autor = "Rafael Almeida";

            db.SaveChanges();

            Console.ReadKey();

            db.Books.Add(
                new Book
                {
                    Titulo = "Dominando o entity framework core",
                    Autor = "Rafael Almeida"
                });

            db.SaveChanges();

            transacao.Commit();
        }
    }
    static void ComportamentoPadrao()
    {
        CadastrarLivro();

        using (var db = new ApplicationContext())
        {
            var livro = db.Books.FirstOrDefault(p => p.Id == 1);
            livro.Autor = "Rafael Almeida";

            db.Books.Add(
                new Book
                {
                    Titulo = "Dominando o entity framework core",
                    Autor = "Rafael Almeida"
                });

            db.SaveChanges();
        }
    }
    static void CadastrarLivro()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Books.Add(
                new Book
                {
                    Titulo = "Introdução ao entity framework core",
                    Autor = "Rafael"
                });

            db.SaveChanges();
        }
    }
    */
    #endregion

    #region MODULO UDFs
    /*
    static void FuncaoDefinidaPeloUsuario()
    {
        CadastrarLivro();

        using var db = new ApplicationContext();

        db.Database.ExecuteSqlRaw(@"
            CREATE FUNCTION ConverterParaLetrasMaiusculas(@dados VARCHAR(100))
            RETURNS VARCHAR(100)
                BEGIN
                    RETURN UPPER(@dados)
                END");

        var resultado = db.Books.Select(p => MinhasFuncoes.LetrasMaiusculas(p.Titulo));
        foreach (var parteTitulo in resultado)
        {
            Console.WriteLine(parteTitulo);
        }
    }
    static void FuncaoLEFT()
    {
        CadastrarLivro();

        using var db = new ApplicationContext();

        //var resultado = db.Books.Select(p => ApplicationContext.Left(p.Titulo, 10));
        var resultado = db.Books.Select(p => MinhasFuncoes.Left(p.Titulo, 10));
        foreach (var parteTitulo in resultado)
        {
            Console.WriteLine(parteTitulo);
        }
    }
    */
    #endregion

    #region MODULO PEFORMANCE
    static void ConsultaRastreada()
    {
        using var db = new ApplicationContext();

        var funcionarios = db.Funcionarios.Include(p => p.Department).ToList();
    }
    static void ConsultaNaoRastreada()
    {
        using var db = new ApplicationContext();

        var funcionarios = db.Funcionarios.AsNoTracking().Include(p => p.Department).ToList();
    }

    static void ConsultaComResolucaoDeIdentidade()
    {
        using var db = new ApplicationContext();

        var funcionarios = db.Funcionarios
            .AsNoTrackingWithIdentityResolution()
            .Include(p => p.Department)
            .ToList();
    }

    static void ConsultaCustomizada()
    {
        using var db = new ApplicationContext();

        db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll; //Configurando Tracking no método enquanto o a instância do contexto for válida.

        var funcionarios = db.Funcionarios
            //.AsNoTrackingWithIdentityResolution()
            .Include(p => p.Department)
            .ToList();
    }

    static void Setup()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Departamentos.Add(new Department
        {
            Description = "Departamento Teste",
            Active = true,
            EmployeeList = Enumerable.Range(1, 100).Select(p => new Employee
            {
                CPF = p.ToString().PadLeft(11, '0'),
                Name = $"Funcionando {p}",
                RG = p.ToString()
            }).ToList()
        });

        db.SaveChanges();
    }
    #endregion
}