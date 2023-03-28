using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEFCore.Domain
{
    public class Document
    {
        private string _cpf;
        public int Id { get; set; }
        public void SetCPF(string cpf)
        {
            //Validações
            if (string.IsNullOrWhiteSpace(cpf))
            {
                throw new Exception("CPF invalido.");
            }
            _cpf = cpf;

        }
        public string GetCPF() => _cpf;

        [BackingField(nameof(_cpf))]
        public string CPF => _cpf;
    }
}
