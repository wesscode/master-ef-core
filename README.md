# master-ef-core
demo master EntityFrameworkCore6.

Microsoft.EntityFrameworkCore.SqlServer => habilita uso do sqlserver com o entity na minha aplicação.
Microsoft.EntityFramewoekCore.Tools => (superset)habilita fazer os comandos de migrações, bagagem de comandos.
Microsoft.EntityFramewoekCore.Design => Gera design da entidade ao executar comando de migração.
instala o cli em modo global => dotnet tool install --global dotnet-ef --version 3.1.5
desinstalar o cli em modo global => dotnet tool unistall --global dotnet-ef --version 3.1.5

## Comandos para instalar os pacotes via CLI 👇
dotnet add package Microsoft.EntityFramewoekCore.Design --version 6.0.9
dotnet add package Microsoft.EntityFramewoekCore.SqlServer --version 6.0.9
dotnet add package Microsoft.EntityFramewoekCore.Tools --version 6.0.9

## Instalar via NUGET vscommunity👇
Menu superior > Tools > Nuget Package Manager > Manager nuget package for solution > Browser >
Pesquise os devidos pacotes.👇

Microsoft.EntityFramewoekCore.Design
Microsoft.EntityFramewoekCore.SqlServer
Microsoft.EntityFramewoekCore.Tools

## Propriedades de navegação
Carregamento adiantado => (.Include())
carregamento explícito => ( carregamento em um momento posterior. )
carregamento lento => ( dados relacionados são carregados por demanda, quando a propriedade de navegação for acessado.)
ex: Model: Solicitation > SolicitationItem(propriedade de navegação)

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