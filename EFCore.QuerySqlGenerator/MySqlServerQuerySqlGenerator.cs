using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace EFCore.QuerySqlGenerator
{
    public class MySqlServerQuerySqlGenerator : SqlServerQuerySqlGenerator
    {
        //Injetado no construtor as infraestrutura que ele precisa, para gerar os comandos sql
        public MySqlServerQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies, IRelationalTypeMappingSource typeMappingSource) : base(dependencies, typeMappingSource)
        {
        }

        //quando for "visitar" uma tabela
        protected override Expression VisitTable(TableExpression tableExpression)
        {
            var table = base.VisitTable(tableExpression);
            Sql.Append(" WITH (NOLOCK)");

            return table;
        }
    }
}
