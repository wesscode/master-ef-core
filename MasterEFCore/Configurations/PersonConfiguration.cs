using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterEFCore.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder
               .ToTable("Pessoas")
               .HasDiscriminator<int>("TipoPessoa")
               .HasValue<Person>(3)
               .HasValue<Instructor>(6)
               .HasValue<Student>(99);
        }
    }
}
