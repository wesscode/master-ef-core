using Microsoft.EntityFrameworkCore;

namespace EFCore.Tips.Domain
{
    [Keyless]
    public class UsuarioFuncao
    {
        public Guid UsuarioId { get; set; }
        public Guid FuncaoId { get; set; }

    }
}
