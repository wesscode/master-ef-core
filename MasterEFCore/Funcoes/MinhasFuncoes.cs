using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Funcoes
{
    public  class MinhasFuncoes
    {
        [DbFunction(name: "LEFT", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }

        public static string LetrasMaiusculas(string dados)
        {
            throw new NotImplementedException();
        }

        public static void RegistrarFuncoes(ModelBuilder modelBuilder)
        {
            var funcoes = typeof(MinhasFuncoes).GetMethods().Where(p => Attribute.IsDefined(p, typeof(DbFunctionAttribute)));

            foreach (var funcao in funcoes)
            {
                modelBuilder.HasDbFunction(funcao);
            }
        }
    }
}
