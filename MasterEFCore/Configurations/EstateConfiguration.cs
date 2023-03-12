using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Configurations
{
    public class EstateConfiguration : IEntityTypeConfiguration<Estate>
    {
        public void Configure(EntityTypeBuilder<Estate> builder)
        {
            builder
                .HasOne(g => g.Governador) //um
                .WithOne(e => e.Estate) //para um
                .HasForeignKey<Governador>(p => p.EstateId); //required se a fk no model n estiver seguindo os padrões de nomeclatura que o entity entende.

            builder
                .Navigation(x => x.Governador)
                .AutoInclude(); //após essa config, não precisa criar .include explicitamente na consulta.
        }
    }
}
