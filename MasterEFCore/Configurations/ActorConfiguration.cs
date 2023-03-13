using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterEFCore.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder
            .HasMany(x => x.MovieList)
            .WithMany(x => x.ActorList)
            .UsingEntity(p => p.ToTable("ActorsMovies")); //Definir nome tabela relacional.
        }
    }
}
