using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace MasterEFCore.Interceptadores
{
    public class InterceptadorDeComandos : DbCommandInterceptor
    {
        private static readonly Regex _tableRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH\(NOLOCK\)))",
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase |
                RegexOptions.Compiled);

        //Método executa antes de executar o comado na base de dados
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            //Console.WriteLine("[Sync] Entrei dentro do método ReaderExecuted");
            //return base.ReaderExecuting(command, eventData, result);

            UsarNoLock(command);
            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            //Console.WriteLine("[Async] Entrei dentro do método ReaderExecuted");
            //return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);

            UsarNoLock(command);
            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        private static void UsarNoLock(DbCommand command)
        {
            if (!command.CommandText.Contains("WITH (NOLOCK)") && command.CommandText.StartsWith("-- Use NOLOCK")) //startWith setado em tag no program.cs
            {
                command.CommandText = _tableRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
            }
        }
    }
}
