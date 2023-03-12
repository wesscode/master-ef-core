using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterEFCore.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.OwnsOne(x => x.Adress, end =>
            {
                end.Property(p => p.District).HasColumnName("District"); //Seleciono a property para da um nome de coluna.

                end.ToTable("Adress"); //Criar tabela adress com fk para client. Sem essa config Adress vira props em client.
            });
        }
    }
}
