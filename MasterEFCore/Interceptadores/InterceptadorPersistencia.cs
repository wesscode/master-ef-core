using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MasterEFCore.Interceptadores
{
    public class InterceptadorPersistencia : SaveChangesInterceptor
    {
        //EXECUTA ANTES DE PESISTIR NA BASE DE DADOS
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);

            return result;
        }
    }
}
