using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFCore.QuerySqlGenerator
{
    public class MyInterceptor : IObserver<KeyValuePair<string, object>> //interceptor responsavel por manipular a informação
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

    public class MyInterceptorListener : IObserver<DiagnosticListener> //listener responsavel ouvir os eventos gerados pelo o efcore.
    {
        private readonly MyInterceptor _interceptor = new MyInterceptor();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == DbLoggerCategory.Name) //momento que identifico o namespace que quero me inscrever.
            {
                listener.Subscribe(_interceptor);
            }
        }
    }
}
