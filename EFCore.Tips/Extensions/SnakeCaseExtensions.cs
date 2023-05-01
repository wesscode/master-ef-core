using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Tips.Extensions
{
    public static class SnakeCaseExtensions
    {
        public static void ToSnakeCaseNames(this ModelBuilder modelBuilder)
        {
            //buscando tipos de entidades
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //buscando e setando o novo padrão de escrita da tabela.
                var tableName = entity.GetTableName().ToSnakeCase();
                entity.SetTableName(tableName);

                foreach (var property in entity.GetProperties())
                {
                    //recupera o identificado da tabela
                    var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, null);

                    //busco o nome da coluna baseada no identificador e seto o padrão de escrita.
                    var columnName = property.GetColumnName(storeObjectIdentifier).ToSnakeCase();
                    //seto o novo nome com a nova escrita.
                    property.SetColumnName(columnName);
                }

                foreach (var key in entity.GetKeys())
                {
                    //obtendo as key e setando o novo padrão
                    var keyName = key.GetName().ToSnakeCase();
                    key.SetName(keyName);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    //obtendo as FK e setando o novo padrão
                    var foreignKeyName = key.GetConstraintName().ToSnakeCase();
                    key.SetConstraintName(foreignKeyName);
                }

                foreach (var index in entity.GetIndexes())
                {
                    //obtendo os indices e setando o novo padrão
                    var indexName = index.GetDatabaseName().ToSnakeCase();
                    index.SetDatabaseName(indexName);
                }
            }
        }

        //UserId -> user_id
        //undelines entre palavras e tudo em caixa baixa.
        private static string ToSnakeCase(this string name)
            => Regex.Replace(
                name,
                @"([a-z0-9])([A-Z])",
                "$1_$2").ToLower();
    }
}