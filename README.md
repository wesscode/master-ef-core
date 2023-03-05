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

## Modulo Modelo de dados:
- Collations: 
   * A forma que banco de dados interpreta os dados.
- Sequences: 
   * Sequencia de registros customizadas via efcore.
   * tipo suportados: int, long, byte, decimal.
   * Drop sequence Name_Sequence
   * Consultar sequences sqlserver: select * from sys.sequence .
- Indices:
  * simples e compostos
  * aplicar filtro para criar o indice
  * aplicar fator de preenchimento
  * definir se o index é UNICO
- Propagação de dados:
  * Pode ser feita em tempo de execução ou tempo migrações.
  * HasData habilita Indentity insert, onde posso passar valor pra minha chave primaria mesmo a mesma sendo auto increment.
- Esquema:
  * Definir esquema no banco de dados.
  * Aplica-se globalmente ou especificamente no onmodelcreating. 
- Conversores de valor :
  * Capacidade de um tipo de dado na classe, e armazena no banco com outro tipo.
  * Capacidade de converter ao inserir e coverter ao buscar dados.
  * Existe vários conversores pré definido no using Microsoft.EntityFrameworkCore.Storage.ValueConversion
  * ValueConverter<> da a possibilidade de fazer conversores customizados.
- Propriedade de sombra:
  * Ao Omitir a FK e deixar somente a prop de navegação o entity entende o relacionamento e cria a coluna DepartamentId
  * Configurando fluent api shadow property
  * Insert e consulta Shadow Property