using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace eCommerce.API.Models
{
    [Table("Usuarios")]

    public class Usuario2
    {
        [Key]


        //**ATRIBUTOS E GETTERS E SETTERS
        //>>>*** MUDANCA DE NOMES DE COLUNAS ***<<<
        //??COMO RESOLVER ESTE PROBLEMA??
        public int Cod { get; set; } //Nome era: Id
        public string NomeCompleto { get; set; } //Nome era: Nome
        public string Email { get; set; }
        public string Sexo { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string NomeCompletoMae { get; set; } //Nome era: NomeMae
        public string Situacao { get; set; } //Nome era: cadastroSituacao
        public DateTimeOffset DataCadastro { get; set; }

        [Write(false)]
        public Contato Contato { get; set; }

        [Write(false)]
        public ICollection<EnderecoEntrega> EnderecosEntrega { get; set; }                                                                     

        [Write(false)]
        public ICollection<Departamento> Departamentos { get; set; }

    }
}
