using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFCore.QuerySqlGenerator
{
    public class MyDiagnosticSource : IObserver<KeyValuePair<string, object>>
    {
        private static readonly Regex _tableAliasRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))", //quando encontrar o alias da nossa tabela vai concatenar com o hints
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase |
                RegexOptions.Compiled);

        //executa qnd completa
        public void OnCompleted()
        {          
        }

        //executa qnd ocorre um erro
        public void OnError(Exception error)
        {            
        }

        //executa qnd chama a proxima acao
        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;

                if (!command.CommandText.Contains("WITH (NOLOCK)"))
                {
                    command.CommandText = _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
                }
            }
        }
    }
}
