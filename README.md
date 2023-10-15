# Master EF Core
demo master Entity Framework Core.

- Microsoft.EntityFrameworkCore.SqlServer => habilita uso do sqlserver com o entity na minha aplicação.
- Microsoft.EntityFramewoekCore.Tools => (superset)habilita fazer os comandos de migrações, bagagem de comandos.
- Microsoft.EntityFramewoekCore.Design => Gera design da entidade ao executar comando de migração.
- instala o cli em modo global => dotnet tool install --global dotnet-ef --version 3.1.5
- desinstalar o cli em modo global => dotnet tool unistall --global dotnet-ef --version 3.1.5

## Comandos para instalar os pacotes via CLI 👇
- dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.9
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.9
- dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.9

## Instalar via NUGET vscommunity👇
- Menu superior > Tools > Nuget Package Manager > Manager nuget package for solution > Browser >
Pesquise os devidos pacotes.
  * Microsoft.EntityFramewoekCore.Design
  * Microsoft.EntityFramewoekCore.SqlServer
  * Microsoft.EntityFramewoekCore.Tools
  * 
## Propriedades de navegação
- Carregamento adiantado => (.Include())
- Carregamento explícito => ( carregamento em um momento posterior. )
- Carregamento lento => ( dados relacionados são carregados por demanda, quando a propriedade de navegação for acessado.)
- ex: Model: Solicitation > SolicitationItem(propriedade de navegação)

## Módulo Modelo de dados:
- Collations: 
   * A forma que banco de dados interpretar os dados.
- Sequences: 
   * Sequência de registros customizadas via efcore.
   * tipo suportados: int, long, byte, decimal.
   * Drop sequence Name_Sequence
   * Consultar sequences sqlserver: select * from sys.sequences
- Índices:
  * simples e compostos
  * aplicar filtros para criar o índice
  * aplicar fator de preenchimento
  * definir se o index é ÚNICO
- Propagação de dados:
  * Pode ser feita em tempo de execução ou em migração.
  * HasData habilita Indentity insert, onde posso passar valor pra minha chave primaria mesmo a prop sendo auto increment.
- Esquema:
  * Definir esquema no banco de dados.
  * Aplica-se globalmente ou especificamente no onmodelcreating. 
- Conversores de valor:
  * Capacidade de um tipo de dado na classe, e armazena no banco com outro tipo.
  * Capacidade de converter ao inserir e coverter ao buscar dados.
  * Existe vários conversores pré definido no using Microsoft.EntityFrameworkCore.Storage.ValueConversion
  * ValueConverter<> da a possibilidade de fazer conversores customizados.
- Propriedade de sombra/Shadow Property:
  * Ao Omitir a FK e deixar somente a prop de navegação o entity entende o relacionamento e cria a coluna relacional(DepartamentId)
  * Configurando fluent api shadow property
  * Insert e consulta Shadow Property
- Owned Types/Tipos de propriedade
  * Configurada no FluentAPI, criando um modelo de dado(classe) complexa.
  * EX: Referência classeA a ClasseB onde a ClasseB passa a ser propriedades da classeA.
  * Config .ToTable("ClasseB") é criada classe com as prop com fk para ClasseA. Tabela essa criada pelo o entity com o conceito Shadow Property.
- Relacionamento 1 Para 1
  * Configurando entidade FluentApi em outro Arquivo
  * IEntityTypeConfiguration
  * AutoInclude
- Relacionamento 1 Para muitos.
  * Configurando relacionamentos na fluent Api
  * Conhecendo OnDelete(), IsRequired()
- Relacionamento Muitos para Muitos.
  * Configurando relacionamentos na fluent Api
  * Customizando nome tabela, nome coluna tabela relacional
  * Adicionando colunas na tabela relacional utilizando o conceito ShadowProperty.
- Configurando Modelo de dados com TPH(Tabela por hierarquia)
  * Quando temos uma entidade como principal e outras entidades herdam dela com campos adicionais.
  * Entity cria uma tabela só com campo adicional(Discriminator), na tabela principal para identificar cada registro.
  * Não é possível definir propriedades obrigatórias para tipos derivados, sendo necessário que todas as prop que não são da classe base, sejam nullable.
- Configurando Modelo de dados com TPT(Tabela por tipo)
  * Tem como objetivo criar uma tabela base e uma tabela específica para cada modelo de dado.
  * Consigo colocar restrição especifícas para cada Entidade.
  * Cria tabela para cadas entidade com fk para a tabela principal.
- Sacola de Propriedades
  * Tem como objetivo compartilhar um tipo de Clr.

## Módulo Atributos - DataAnnotations:
  * DataAnnotations é uma subscrita da FluentAPI
  * Configura o modelo de dados com anotações
  * Alguns atributos, Required, Table, Column,TypeName, MaxLength, Key
- Atributo InverseProperty
  * Permite identificar ao entity com qual fk o atributo de sua tabela de deve se relacionar.
  * Atributo esse que pode ser ultilizado tanto na classe dependente como na classe principal, seria equivalente ao HasOne().HasMany(property); no fluent api.
- Atributo NotMapped
  * Entity ignora classe ou prop que possuir essa anotação, logo não gerando no banco.
- Atributo DatabaseGenerated
  * Geralmete aplicada ao campo Key.
  * Para configurar se o campo vai ser identity, None, Computed.
  * Computer: o campo n recebe valor nem no insert nem no update, somente leitura.
- Atributo Index
  * Criar index simples ou composto unico via anotation.
- Atributo Comment
  * Quando adicionado sobre a entitade um comentário é adicionado na tabela. Também pode colocar comment para um determinado campo.
- Atributo BackingField
  * Campo de apoio.
- Atributo Keyless
  * Quando o entity estiver fazendo o mappeamento do modelo de dados ele não considere a chave primaria.
  * Consultar dados de uma tabela que não possui chave ou seja tabelas de projeções ou views. ex consulta de relatório etc.  
  * Não rastreadas pelo o entity, então não pode adicionar dados.

## Módulo EF Functions:
  * As EF functions so pode ser utilizado somente em consultas onde o entity vai traduzir para suas consultas.
- Função de datas
  * DateDiffDay, DateFromParts, IsDate, DateDiffMonth etc..
- Like
- DataLength
  * Conta quantidade de bytes armazeados.
  * se o campo for nvarchar(unicode) o mesmo armazena 2 bytes para cada caracter.
- Property
- Collate
  * Difereciar maiúsculas de minúsculas em uma consulta.

## Módulo Interceptação: 
- IDbCommandInterceptor
  * Implementa métodos no momento em que o comando está sendo criado ou sendo executado o comando no banco de dados.
- IDbConnectionInterceptor
  * Fornece métodos no momento que está abrindo uma conexão, que está fechando uma conexão. pode manipular a abertura de conexão.
- IDbTransactionInterceptor
  * Fornece o momento exato que está sendo criado uma transação ou quando está utilizando uma transação existente ou revertendo uma transação.
- OptionsBuilder.AddInterceptors(new InterceptadorDeComandos()), Registrando o intercept o pipeline do entity. 
- Sobrescrevendo métodos da classe base
- Aplicando hint NOLOCK nas consultas
- Interceptando abertura de conexão com banco de dados
- Interceptando alterações
  * Remover, alterar, inserir

## Módulo Transações
  * Sigla(ACID): Atomicidade, Consistencia, Isolação, Durabilidade.
  * A: faz tudo ou não faz nada(bem resumido)
  * C: Garantir que o banco esteja consistente antes e depois da transação
  * I: Uma trasação em andamento deve permanecer totalmente isolada das outras operações. 
  * D: Garantir que as transações executadas sejam gravadas no banco, salvando-as em permanentemente até o serviço do banco voltar e só ai executar as pendências.
- Comportamento padrão EFCore
  * todas as operações que são executadas no banco após chamar o savechages por padrão é executada dentro de uma transação.
- Gerenciando transação manualmente
- Revertendo uma transação
- Salvando ponto de uma transação
- Usando TransactionScope
  * Classe que possui sua própria instrutura para gerenciar as transações automaticamente, sendo assim o transactionScope, diminui a complexidade do código. 
- Ferramentas:
  * SQL Profile: Ferramenta para monitorar todos os comandos que estão sendo enviados para sua base de dados.

## Módulo UDF'S
  * Função Definida pelo o usuário.
- Built-In Function
  * Função incorporada no banco de dados.
- Registrando Funções via Data Annotations
- Registrando funções via Fluent API
- Função definida pelo o usuário
- Customizando uma função  
## Módulo Performance
- Tracking 
  * Consulta traqueada o entity gerar uma instâcia local da entidade em cache e reutiliza quando pra evitar idas ao banco de informações que você já trouxe.
- AsNoTracking
  * Consulta não traqueada o entity não gera instâcia, so retorna e o ciclo de vida se encerra. Indicada para consultas de leituras.
- AsNoTrackingWithIdentityResolution  
  * Consulta não traqueada com resolução, indicada para gerenciar memória evitando explosão cartesiaa quando fazer uma consultas não traqueadas onde a informação de uma tabela se repete nos demais.
  * Exemplo consulta de N => 1
- Configurando Tracking de forma global
  * UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution) no OnModelCreating
- Configurando Tracking no método enquanto o instância do contexto for válida.
  * db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
## Módulo Migrations
- Microsoft.EntityFrameworkCore.Design
  * Possui um conjunto de metódos responsaveis e capaz de criar arquivos de migrações e gerar scripts que serão executados no banco de dados.
- Microsoft.EntityFrameworkCore.Tools
  * Possui um super set de comandos powershell para executar e aplicar as migrações no banco de dados.
- Global Tool EntityFrameworkCore 
  * Instalar o EF CLI de forma global para aplicar as migrações via prompt de comando.
  * dotnet tool install --global dotnet-ef --version 5.0.3
- ARQUIVOS DE MIGRAÇÃO
- PrimeiraMigracao.cs
  * Arquivo contém todas as ações que acontecerão no banco de dados.
  * Metódo UP: mudanças que estão acontecendo. Podendo customizar e colocar consultas etc.
  * Metódo DOWN: Reverte o up
- PrimeiraMigracao.Design.cs
  * Possui todas as configurações e metadados, cópia do modelo de dados no momento da geração da migração com as alterações recente no modelo de dados.
- ApplicationContextSnapshot.cs
  * Foto do estado atual do modelo de dados, atualizado a cada migração criada. Permite o efcore identifique as mudanças necessárias.
- Gerando Script de migração
  * dotnet ef migrations script -p CaminhoProjeto -o CaminhoOndeDesejaCriarScript
  * obs: verificar se gera sempre a ultima migração criada, ou de todas as migrações.
- Gerando scripts de migração Idempotentes
  * Evitar problemas de concorrência aplicando validações de existencia no script gerado.
  * dotnet ef migrations script -p CaminhoProjeto -o CaminhoDesejaCriarScriptIdempotente.SQL -i
- Aplicando Migrações no banco de dados
  * 1° e mais segura: pegando o script sql, entregando a um dba para que o mesmo possa aplicar no banco.
  * 2° aplicando via prompt de comando: dotnet ef database update -p CaminhoProjeto -v
  * 3° em tempo de execução instanciando o contexto da aplicação e executando o comando db.Database.Migrate();
- Desfazendo migrações
  * dotnet ef database update NomeDaMigracaoAteOndeQuerReverter
  * Se passar nome de uma migração após o comando UPDATE logo o entity entenderá e dará rollback/reverter as migrações aplicadas no banco até a migração informada.
  * Migrações desfeitas os arquivos físicos ficam como PENDENTES, logo pode se removidas através do comando: dotnet ef migrations remove.
- Verificando Migrações Pendentes
  * 1° via prompt de comando: dotnet ef migrations list
  * 2° em tempo de execução instanciando o contexto da aplicação e executando o comando db.Database.GetPendingMigrations();
- Engenharia Reversa/Scafollding
  * Processo utilizado para criar um modelo de dados no EFCore, com base em um banco de dados já EXISTENTE.
  * Banco precisa está bem modelado.
  * Começa lendo os schemas, ler as informações das tabelas existentes, colunas, restrições e por fim os indices.
  * Com base nas informações coletadas o EFCore cria suas classes com suas relações e o CONTEXTO.

  * COMANDO: 
    dotnet ef dbcontext scafolld "StringDeConexao" Microsoft.EntityFrameworkCore.SqlServer
    --table Pessoas --use-database-names --data-annotations --context-dir .\PastaOndeQueroPorOContexto
    --output-dir .\LocalOndeQueroQueGereMinhasEntidades --namespace Meu.NameSpace
    --context-namespace Meu.NameSpace.Contexto

  * Entendo prefixos do comando:
  * --table TabelaX: Especificando qual tabela vc quer gerar no scafolld, se tiver mais uma tabela o comando deve ser repetido para cada tabela.
  * --use-database-names: Força o EFCore preservar o nome de tabelas, colunas, indices o max possível na hora do scafolld.
  * --data-annotations: Especificando para que o efcore utilize o max possivel com dataAnnotations na modelagem das classes.
  * --context-dir: Especifica o diretório que o contexto deve ser criado.
  * --output-dir: Especifica o diretório onde deve ser criado o modelo de dados(entidades).
  * --namespace: Especifica o namespace para as ENTIDADES.
  * --context-namespace: Especifica o namespace para o CONTEXTO.

# EFCore.Tips
  Conhecendo dicas e truques com efcore.

## Módulo Dicas e Truques 
- ToQueryString()
  * Método de extensão utilizado para obter a instrução SQL pura que será executada apartir de uma instrução Linq.
- DebugView
  * Conhecendo auxiliar DebugView, ao debugar o código em Locals, terá variavel _db_ e _query_ dentro de cada uma possui a variavel DebugView que terá informações do obj e a query que irá executar.
- Redefinir estado do Contexto.
  * db.ChangeTracker.Clear()
  * Limpar objetos que estão sendo rastreado pelo o contexto sem precisar instanciar um novo contexto.
- Include com consulta filtrada.
  * Aplicar filtros nas propriedades de navegação.
- SingleOrDefault vs FirstOrDefault
  * SingleOrDefault: Retorna um único elemento de uma sequência ou um valor padrão se esse elemento não for encontrado. Ocorrerá exceção quando existir mais de um elemento no resultado da consulta.
  * Sendo utilizado mais em regras de negócio.

  * FirstOrDefault: Retorna o primeiro elemento de uma sequência ou um valor padrão se nenhum elemento for encontrado.
  * Sendo utilizado em consultas simples.
- Consulta em tabelas sem chave primária
  * EFCore consegue gereciar migrações com esse tipo de tabela.
  * O recurso é limitado a somente CONSULTAS logo não podendo persistir dados na mesma.
- Usando views de seus bancos de dados
  * view é um tipo de exibição de dado criada atráves do resultado de uma consulta.
  * atráves o da aotação .ToView("nome-da-view") na fluent api, o efcore entende que a entidade que está sendo exposta não deve ser gerada uma tabela e sim cosumir uma view existente.
  * após a entidade mapeada para uma view podemos executar consultas na mesma aplicado filtros etc, consumindo a view mappeada.
- Forçando o uso do VARCHAR
  * atributo string que não foi mappeado tamanho, por default o efcore cria a prop no banco como nvarchar(MAX).
  * nvarchar é o padrão Unicode e armazena o dobro de bytes do tipo varchar.
  * Fazendo filtro dos attr que não possuem configuração mappeada conseguimos aplicar e forçar o entity a criar essas props com VARCHAR.
- Aplicando conversão de nomenclatura
  * Definindo padrão de nomeclatura do banco com efcore.
  * Nome de tabela, indices, colunas, pk's, fk's.
- Operadores de agregacao
- Operadores de agregacao no agrupamento
- Contadores de eventos no DOTNET
  * pacote dotnet-counters
  * Esse pacote permite executar um processo que monitora nossa aplicação, no exemplo estamos monitorando o Efcore, seus eventos e contextos que estão em execução na aplicação.
  * Na aplicação pegar o PID do processo.
  * em modo debug abri um console com o seguinte comando na raiz do projeto.
    * dotnet counters monitor -p {PID} --counters Microsoft.EntittFrameworkCore.

# EFCore.Testes
Criando testes utilizando provider InMemory e SQLite com EFCore.

## Testes
- Criando testes usando provider InMemory
  * InMemory so armazena dados temporários, logo quando o contexto é descartado todos os dados tbm serão descartados.
  * Provider InMemory Possui limitações que não da pra ser aplicado nos testes    
    * calculos e datas: EF.Functions, so funciona no provider SqlServer, pq são funções executadas pelo o próprio banco.
- Criando testes usando provider SQLite.  

# EFCore.SqlServerGenerator
Sobrescrevendo comportamentos do efcore no momento em que está produzindo as query no banco de dados. 

## Módulo Sobrescrevendo comportamentos do EFCore
- Gerador de SQL customizado.
- Criando Factory do gerador customizado.
- Usando o gerador de SQL customizado.
  * Customizando o comportamento default do efcore, podemos customizar ao visitar uma table, coluna, alterar o script gerado, interceptando e adicionando hints nas instruções diretamente na factory de criação interna do entity.

## Módulo Diagnostic Source
Fazer disgnostico de qualquer ferramenta que é utilizado no ecosistema dotnet. Exemplo as atividades do efcore, entender como é realizada internamente. Entender como criar um observador e se assinar no pipeline diagnostic source de frameworks utilizados no dotnet.

- É um padrão observer que tem como papel principal notificar os assinador do pipeline. Que por sua vez o efcore faz um bom uso dele.
- Criando um interceptador para o Diagnostic Source
- Criar um listener
- Assinando o listener e validando.
  * Diagnostic Source não fica limitado em ouvir somente logs do entity e sim de demais ferramentas do dotnet, para ta contruindo uma lib de logs. 
