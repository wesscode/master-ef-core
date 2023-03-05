using System.Net;

namespace MasterEFCore.Domain
{
    public class Conversor
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public Versao Versao { get; set; }
        public IPAddress IPAdress { get; set; }
        public Status Status { get; set; }
    }

    public enum Versao
    {
        EFCore1,
        EFCore2,
        EFCore3,
        EFCore4,
        EFCore5
    }

    public enum Status
    {
        Analise,
        Enviado,
        Devolvido
    }
}
