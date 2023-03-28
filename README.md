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