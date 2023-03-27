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
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
       public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder
                .Property("_cpf").HasColumnName("CPF").HasMaxLength(11);
                
                //.Property(p => p.CPF) //essa é a prop
                //.HasField("_cpf"); //esse é o campo que vai ser criado, indicando qual campo popular qnd consultar o dado.
        }
    }
}
