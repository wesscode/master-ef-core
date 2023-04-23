using System;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Tips.Domain
{
    public class DepartamentoRelatorio
    {
        public string Departamento { get; set; }
        public int Colaboradores { get; set; }
    }
}