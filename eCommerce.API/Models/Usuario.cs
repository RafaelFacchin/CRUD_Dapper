using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace eCommerce.API.Models
{
    //!!!!INDICACAO DA TABELA DE DADOS P/ PODER INFORMAR O OBJETO(tabela) AO DAPPER.CONTRIB!!!!
    [Table("Usuarios")]//Data Anotation: Informa a TABELA (ORM) PARA FUNCIONAMENTO DO Dapper.Contrib

    public class Usuario
    {
        [Key]//data anotation p/ informar a CHAVE PRIMARIA(PK) DA TABELA
        
        
        //**ATRIBUTOS E GETTERS E SETTERS
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public string  RG { get; set; }
        public string CPF { get; set; }
        public string NomeMae { get; set; }
        public string SituacaoCadastro { get; set; }
        public DateTimeOffset DataCadastro { get; set; } //TIP: DateTimeOffset inclui a data e hora de registro, alem de registrar o FUSO HORARIO (GMT)

        //DEIFINICAO DE RELACIONAMENTOS ENTRE AS TABELAS
        //Tabela usuario (classe usuario) com tabela Contato (classe Contato), Enderecos de entrega       
        //!!Composicao de um relacionamento entre tabelas e classes
        
        [Write(false)]//O DAPPER considera que apos este DATA ANOTTATION nao considera estes campos (abaixo)
        public Contato Contato { get; set; }//Relacionamento 1:1; Toda vez que a classe usuario for instanciada, trarah tb a classe Contato

        [Write(false)]
        public ICollection<EnderecoEntrega> EnderecosEntrega { get; set; }//Relacionamento 1:M (um usuario pode ter mais de um endereco de entrega); Toda vez que a classe usuario for instanciada, trarah tb a classe EnderecoEntrega                                                                          //TIP: o ICollection, retorna uma LISTA com os enderecos de entrega do usuario

        [Write(false)]
        public ICollection<Departamento> Departamentos { get; set; }//Relacionamento M:M (um usuario pode comprar em VARIOS DEPARTAMENTOS, assim como VARIOS departamentos vendem para VARIOS Usuarios); Toda vez que a classe usuario for instanciada, trarah tb a classe Departamento
                                                                          //TIP: o ICollection, retorna uma LISTA com os nomes dos DEPARTAMENTOS

    }
}
