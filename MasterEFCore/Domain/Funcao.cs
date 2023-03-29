using System.ComponentModel.DataAnnotations.Schema;

namespace MasterEFCore.Domain
{
    public class Funcao
    {
        public int Id { get; set; }
        [Column(TypeName ="NVARCHAR(100)")]
        public string Description1 { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string Description2 { get; set; }
        public DateTime Data1 { get; set; }
        public string Data2 { get; set; }
    }
}