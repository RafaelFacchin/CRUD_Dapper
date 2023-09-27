namespace eCommerce.API.Models
{
    public class EnderecoEntrega
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeEndereco { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }

        //DEIFINICAO DE RELACIONAMENTOS ENTRE AS TABELAS
        //Tabela Enderecos de entrega (classe EnderecoEntrega) com tabela usuario (classe Usuario) 
        //!!Composicao de um relacionamento entre tabelas e classes
        public Usuario Usuario { get; set; } //Relacionamento 1:1; Toda vez que a classe EnderecoEntrega for instanciada, trarah tb a classe usuario

    }
}
