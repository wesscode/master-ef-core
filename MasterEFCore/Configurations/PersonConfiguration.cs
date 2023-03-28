using MasterEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterEFCore.Configurations
{
    /* 
    * TPH 
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
    }*/

    //TPT
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder
               .ToTable("Pessoas");
        }
    }
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder
               .ToTable("Instrutores");
        }

    }
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
               .ToTable("Alunos");
        }
    }
}