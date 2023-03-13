using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterEFCore.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            /*
             builder
             .HasMany(x => x.MovieList)
             .WithMany(x => x.ActorList)
             .UsingEntity(p => p.ToTable("ActorsMovies")); //Definir nome tabela relacional.
            */

            builder
                .HasMany(x => x.MovieList)
                .WithMany(x => x.ActorList)
                .UsingEntity<Dictionary<string, object>>(
                "MoviesActores", //definindo nome tabela relacional
                p => p.HasOne<Movie>().WithMany().HasForeignKey("MovieId"), //definindo nome coluna
                P => P.HasOne<Actor>().WithMany().HasForeignKey("ActorId"), //definindo nome coluna
                p =>
                {
                    p.Property<DateTime>("CreateAt").HasDefaultValueSql("GETDATE()"); //criando prop utilizando tecnica shadownProperty
                });
        }
    }
}
